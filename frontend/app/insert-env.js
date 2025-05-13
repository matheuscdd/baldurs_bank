const fs = require('fs');
const path = require('path');

const isProd = process.argv[2] === 'prod';
const filename = isProd ? 'environment.prod.ts' : 'environment.ts';

const environment = JSON.stringify({
    production: isProd,
    apiURL: process.env.API_URL,
    firebase: {
        apiKey: process.env.FIREBASE_API_KEY,
        authDomain: process.env.FIREBASE_AUTH_DOMAIN,
        databaseURL: process.env.FIREBASE_DATABASE_URL,
        projectId: process.env.FIREBASE_PROJECT_ID,
        storageBucket: process.env.FIREBASE_STORAGE_BUCKET,
        messagingSenderId: process.env.FIREBASE_MESSAGING_SENDER_ID,
        appId: process.env.FIREBASE_APP_ID,
        measurementId: process.env.FIREBASE_MEASUREMENT_ID,
    }
});

const envConfig = `export const environment = ${environment};`;

const basePath = './src/environments';
if (!fs.existsSync(basePath)) {
    fs.mkdirSync(basePath);
}
fs.writeFileSync(path.join(basePath, filename), envConfig);