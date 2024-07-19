// Directivas de Blazor y servicios relacionados
global using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
global using Microsoft.AspNetCore.Components.Web;

// Servicios de terceros utilizados en la aplicación
global using MudBlazor.Services; // Servicio de MudBlazor para componentes de UI
global using Blazored.SessionStorage; // Servicio para almacenamiento en sesión

// Espacios de nombres relacionados con la aplicación y utilidades propias
global using Entradas.Client; // Espacio de nombres del cliente de la aplicación
global using Entradas.Client.Utils; // Utilidades propias utilizadas en el cliente

// Espacios de nombres compartidos entre el cliente y el servidor
global using Entradas.Shared.Models; // Modelos compartidos entre el cliente y el servidor
global using Entradas.Shared.Wrappers; // Envoltorios compartidos para respuestas de servicio

// DTO (objetos de transferencia de datos) utilizados en la aplicación
global using Entradas.Shared.DTO; // DTOs compartidos entre el cliente y el servidor
global using Entradas.Shared.DTO.EventoDto; // DTO específico para eventos
global using Entradas.Shared.DTO.OrdenDto; // DTO específico para órdenes

// Servicios específicos del cliente utilizados en la aplicación
global using Entradas.Client.Services; // Servicios de cliente comunes
global using Entradas.Client.Services.CategoriaService; // Servicio para gestionar categorías
global using Entradas.Client.Services.AuthService; // Servicio para autenticación
global using Entradas.Client.Services.EventoService; // Servicio para gestionar eventos
global using Entradas.Client.Services.EventoEntradaService; // Servicio para entradas de eventos
global using Entradas.Client.Services.EventoFechaService; // Servicio para fechas de eventos
global using Entradas.Client.Services.OrdenService; // Servicio para órdenes
global using Entradas.Client.Services.BannerService; // Servicio para banners
global using Blazored.LocalStorage;
global using Microsoft.AspNetCore.Components.Authorization;
global using System.Net.Http.Json;
