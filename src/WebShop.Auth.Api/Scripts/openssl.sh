openssl req -newkey rsa:2048 -nodes -keyout WebShop.key -x509 -days 365 -out WebShop.cer
openssl pkcs12 -export -in WebShop.cer -inkey WebShop.key -out WebShop.pfx
