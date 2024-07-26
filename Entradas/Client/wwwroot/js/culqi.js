const settings = {
    title: 'Realizacion de pago',
    currency: 'PEN',
    amount: 600, // Asegúrate de que el monto está en centavos
};

//c2391c29-3b71-4a27-b55e-bac3457ecb1f
/* -----BEGIN PUBLIC KEY-----
MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDSGBntzxDV2WecsK5MicVaXb3J
sz1QFAIO6GYkcsQGic4aug1zYESr06sCzSCPXQT1N1P + uss8CgasPeNtmE + LHm + q
HvvqMv6 + yjzWeNon4QiqbOHvRu4ZwHhNok53c3abnNQ + OufTwcbQv7puOkvAhyIq
OXgb2n / A79IFmX63mwIDAQAB
----- END PUBLIC KEY-----*/

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
    email: 'mario.correa@fabrica.pe',
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

const config = {
    settings,
    client,
    options,
    appearance,
};

const publicKey = 'pk_live_f88f5d1d2457d6af'; 

const Culqi = new CulqiCheckout(publicKey, config);

window.initCulqi = function (buttonId) {
    const btn_pagar = document.getElementById(buttonId);
    if (btn_pagar) {
        btn_pagar.addEventListener('click', function (e) {
            Culqi.open();
            e.preventDefault();
        });
    }
};

window.AbrirCulqi = function () {
    Culqi.open();
};

const handleCulqiAction = async () => {
    if (Culqi.token) {
        const token = Culqi.token.id;
        console.log('Se ha creado un Token: ', token);

        try {
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