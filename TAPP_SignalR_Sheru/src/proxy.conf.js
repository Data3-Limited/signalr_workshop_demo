const PROXY_CONFIG = [
  {
    context: [
        "/weatherforecast",
       "/api/start"
    ],
    target: "https://localhost:7045",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
