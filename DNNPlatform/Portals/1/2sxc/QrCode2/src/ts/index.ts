let QRCode = require('../../node_modules/qrcode-svg');

interface QrCodeOptions {
    color: string;
    height: number;
    width: number;
}

export function init({ domId, options }: { domId: string, options: QrCodeOptions }) {
    let qrDomObject = document.querySelector(`.${domId}`);
    let qrCode = new QRCode({
        content: qrDomObject.getAttribute("href"),
        color: options.color,
        width: options.width,
        height: options.height,
        join: true,
        ecl: "L",
        padding: 0,
    });
    qrDomObject.innerHTML = qrCode.svg();
}

// so it can be called from the HTML when content re-initializes dynamically
const winAny = (window as any);
winAny.appQr2 ??= {};
winAny.appQr2.init ??= init;