# TokenBasedAuthorization
AspNet Core Token based Authorization

To execute and get token from this api you need to simply type below url in browser after executing application from visual studio change localhost port based on your machine.

URL : https://localhost:44353/api/Account?userId=ABC&password=123

Above URL will return Token Copy token and paste the token in Postman Authorization header under Bearer token and paste below url in post url section with GET request.

URL : https://localhost:44353/api/Account/ReadValues

If you try to execute above URL without bearer token you will get unauthorized response from server.


Till now we talk about how to execute application Now will see What is Token based Authorization.

Here we're talking about JWT token stands for Json Web Token. 
1. It is open standard means anyone can use for web application authorization. JWT is certified with RFC 7519.
2. Securely exchange information between server and client with secure signed key
3. JWT can be send wuth the help of URL request, Http header and is it faster.
4. JWT contain information of user encrypted in base64 formate which is verfied on server lever on each http request as Http is stateless protocol.

JWT again divided in 3 parts 
Header, Payload and Signature

Shown below is token return from server once user authenticate the website successfully, Token is encrypted and uderstand by server only client will store token in client storage and pass token in http request, server Read token sent from browser and verify the token and authorize requested resource to browser request.

"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c"

JWTToken(header.payload.signature)

1. Header : Json below from header section decides algorithm(HS256) for signature.It is encoded with base64 format
{
  "alg": "HS256",
  "typ": "JWT"
}

2. Payload: It is again json object which contains information of claims. claims are user details or additional data like expiry date of token ,admin etc.
It is again encoded with Base64 format to form second part.
{
   "sub":"1234567890",
   "name":"Asif S",
   "admin":"true"
}

3. Signature : It is most important part of Token. It combine base64 header and payload with secret key. Secret key declared on server and it is not visible or decode.
Signature is combination base64 format of header, payload and secret key shown below

HMACSHA256( base64urlEncode(header).base64urlEncode(payload), secret key from server).

If clients modify anything token verification failed on server and return unauthorize response.






