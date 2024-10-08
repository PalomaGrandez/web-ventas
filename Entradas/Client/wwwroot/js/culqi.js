/*const settings = {
    title: 'Realizacion de pago',
    currency: 'PEN',
    amount: 10000, // Asegúrate de que el monto está en centavos
};

const client = {
    email: 'soporte@fabrica.pe',
};

*/
const paymentMethods = {
    tarjeta: false,
    yape: true,
    billetera: false,
    bancaMovil: false,
    agente: false,
    cuotealo: false,
};

const options = {
    lang: 'auto',
    installments: true, // Habilitar o deshabilitar el campo de cuotas
    modal: true,
    container: "#culqi-container", // Opcional - Div donde quieres cargar el checkout
    paymentMethods: paymentMethods,
    paymentMethodsSort: Object.keys(paymentMethods), // las opciones se ordenan según se configuren en paymentMethods
};

const appearance = {
    theme: "default",
    hiddenCulqiLogo: false,
    hiddenBannerContent: false,
    hiddenBanner: false,
    hiddenToolBarAmount: false,
    menuType: "sidebar",
    buttonCardPayText: "Pagar 6.00 PEN",
    logo: null,
    defaultStyle: {
        bannerColor: "blue",
        buttonBackground: "yellow",
        menuColor: "pink",
        linksColor: "green",
        buttonTextColor: "blue",
        priceColor: "red",
    },
};

window.AbrirCulqi = function (settings, client) {

    const config = {
        settings: settings,
        client: client,
        options,
        appearance,
    };

    const publicKey = 'pk_live_f88f5d1d2457d6af';

    const Culqi = new CulqiCheckout(publicKey, config);

    const handleCulqiAction = async () => {
        if (Culqi.token) {
            const token = Culqi.token.id;
            const amount = config.settings.amount;
            const email = config.client.email;
            const nombre = config.client.first_name;
            const apellido = config.client.last_name;
            /*const telefono = config.client.phone_number;*/
            console.log('Se ha creado un Token: ', token);
            console.log('El golpe es: ', amount);
            console.log('Su mail del causa es: ', email);
            console.log('el nombre del causa es: ', nombre + " " + apellido);
           /* console.log('el fono del causa es: ', telefono);*/

            try {
                const response = await fetch('https://manageek.com/api/pagos/procesar', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({
                        TokenId: token,
                        Amount: amount,
                        Email: email,
                        Nombre: nombre,
                        Apellido: apellido,
                        /*Telefono: telefono*/
                    }),
                });

               
                
                const result = await response.json();
                console.log('Respuesta del backend: ', result);

                if (response.ok) {

                    DotNet.invokeMethodAsync('Entradas.Client', 'ProcesarPago', token)
                        .then(data => {
                            console.log('Stock actualizado en Blazor.');
                        })
                        .catch(error => {
                            console.error('Error al actualizar el stock en Blazor: ', error);
                        });

                    window.location.href = "/usuario/pago-completo";

                    alert('Pago procesado exitosamente.');
                } else {
                    alert('Error al procesar el pago: ' + result.message);
                }
            } catch (error) {
                console.error('Error en la solicitud de pago:', error);
                alert('Error en la solicitud de pago.');
            }
            Culqi.close();
        } else if (Culqi.order) {
            const order = Culqi.order;
            console.log('Se ha creado el objeto Order: ', order);
        } else {
            console.log('Error : ', Culqi.error);
        }
    };

    Culqi.culqi = handleCulqiAction;

    Culqi.open();
};





