const settings = {
    title: 'Culqi  store 2',
    currency: 'PEN',
    amount: 8000,
}

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

const client = {
    email: 'soporte@fabrica.pe',
};

const appearance = {
    theme: "default",
    hiddenCulqiLogo: false,
    hiddenBannerContent: false,
    hiddenBanner: false,
    hiddenToolBarAmount: false,
    menuType: "sidebar",
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


const handleCulqiAction = async () => {
    if (Culqi.token) {
        const token = Culqi.token.id;
        console.log('Se ha creado un Token: ', token);

        const response = await fetch('https://localhost:7122/api/pagos/procesar', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                TokenId: token
            }),
        });

        const result = await response.json();
        console.log('Respuesta del backend: ', result);

        if (response.ok) {
            alert('Pago procesado exitosamente.');
        } else {
            alert('Error al procesar el pago: ' + result.message);
        }
    } else if (Culqi.order) {
        const order = Culqi.order;
        console.log('Se ha creado el objeto Order: ', order);
    } else {
        console.log('Errorrr : ', Culqi.error);
    }
}

window.AbrirCulqi = function (settings) {

    Culqi.open();
};

const config = {
    settings,
    client,
    options,
    appearance,
};

const publicKey = 'pk_test_3153d0c7ed6a853a'; 

const Culqi = new CulqiCheckout(publicKey, config);

Culqi.culqi = handleCulqiAction;
