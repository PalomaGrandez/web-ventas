﻿using Entradas.Shared.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Entradas.Server.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<bool> CheckEmailExists(string email)
        {
            return await _context.Usuario.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> CheckUsernameExists(string username)
        {
            return await _context.Usuario.AnyAsync(u => u.NombreUsuario == username);
        }
        public async Task<ServiceResponse<int>> Registro(UsuarioRegistroDto request)
        {
            var response = new ServiceResponse<int>();

            // Verificar si el email o nombre de usuario ya existen
            if (await CheckEmailExists(request.Email))
            {
                response.Success = false;
                response.Message = "El email ya está en uso.";
                return response;
            }

            if (await CheckUsernameExists(request.NombreUsuario))
            {
                response.Success = false;
                response.Message = "El nombre de usuario ya está en uso.";
                return response;
            }

            // Crear hash de la contraseña
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var usuario = new Usuario
            {
                Nombres = request.Nombres,
                ApellidoPaterno = request.ApellidoPaterno,
                ApellidoMaterno = request.ApellidoMaterno,
                NombreUsuario = request.NombreUsuario,
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                FechaCreacion = DateTime.Now,
                Rol = "Customer",
                 TipoDocumento = request.TipoDocumento,
                NumeroDocumento = request.NumeroDocumento
            };

            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();

            response.Data = usuario.UsuarioId;
            response.Message = "Usuario registrado exitosamente.";
            return response;
        }

        //Operaciones adicionales
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac
                .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
        private string CreateToken(Usuario usuario)
        {
            List<Claim> claims = new()
            {
                new(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString()),
                new(ClaimTypes.Email, usuario.Email),
                new(ClaimTypes.Role, usuario.Rol),
                new(ClaimTypes.Name, usuario.Nombres),
                new(ClaimTypes.GivenName, usuario.ApellidoPaterno),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        public async Task<ServiceResponse<UsuarioListadoDto>> GetUsuarios(bool paginar = false, int pagina = 1)
        {
            ServiceResponse<UsuarioListadoDto> response = new();

            var usuariosQueryable = _context.Usuario
                                        .Select(x => new UsuarioItemDto
                                        {
                                            UsuarioId = x.UsuarioId,
                                            Nombres = x.Nombres,
                                            ApellidoPaterno = x.ApellidoPaterno,
                                            ApellidoMaterno = x.ApellidoMaterno,
                                            NombreUsuario = x.NombreUsuario,
                                            Email = x.Email,
                                            FechaCreacion = x.FechaCreacion,
                                            TipoDocuemnto = x.TipoDocumento,
                                            NumeroDocumento = x.NumeroDocumento
                                        })
                                        .AsQueryable();
            int resultadosPorPagina = 10;

            if (paginar)
            {
                var registrosTotales = await usuariosQueryable.CountAsync();
                var cantidadPaginas = Math.Ceiling(registrosTotales / (double)resultadosPorPagina);

                var usuarios = await usuariosQueryable
                                        .OrderBy(x => x.UsuarioId)
                                        .Skip((pagina - 1) * resultadosPorPagina)
                                        .Take(resultadosPorPagina)
                                        .ToListAsync();

                UsuarioListadoDto usuarioListadoDto = new()
                {
                    Usuarios = usuarios,
                    PaginaActual = pagina,
                    Paginas = (int)cantidadPaginas,
                    RegistrosTotales = registrosTotales
                };

                response.Success = true;
                response.Data = usuarioListadoDto;
            }
            else
            {
                var usuarios = await usuariosQueryable.ToListAsync();
                response.Success = true;
                response.Data = new UsuarioListadoDto
                {
                    Usuarios = usuarios,
                    PaginaActual = 1,
                    Paginas = 1,
                    RegistrosTotales = usuarios.Count
                };
            }
            return response;
        }

        public async Task<ServiceResponse<string>> Login(UsuarioLoginDto request)
        {
            var response = new ServiceResponse<string>();

            try
            {
                // Check if login identifier is an email or username
                var usuario = await _context.Usuario
                    .FirstOrDefaultAsync(x => x.Email.ToLower() == request.LoginIdentifier.ToLower() ||
                                              x.NombreUsuario.ToLower() == request.LoginIdentifier.ToLower());

                if (usuario == null)
                {
                    response.Success = false;
                    response.Message = "Usuario no encontrado.";
                }
                else if (!VerifyPasswordHash(request.Password, usuario.PasswordHash, usuario.PasswordSalt))
                {
                    response.Success = false;
                    response.Message = "Contraseña incorrecta.";
                }
                else
                {
                    response.Success = true;
                    response.Message = "Inicio de sesión exitoso.";
                    response.Data = CreateToken(usuario);
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error durante el inicio de sesión: " + ex.Message;
            }

            return response;
        }


        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash =
                hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }

        public async Task<ServiceResponse<Usuario>> GetUsuarioById(int usuarioId)
        {
            ServiceResponse<Usuario> response = new();

            var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);
            if (usuario == null)
            {
                response.Success = false;
                response.Message = "No se encontró el usuario";
            }
            else
            {
                response.Data = usuario;
            }
            return response;

        }
    }
}
