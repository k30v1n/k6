# k6

- installation https://github.com/grafana/k6#install

# Usage
- k6 works with the concept of virtual users (VUs) that execute scripts
- Scripts are written using JavaScript as ES6 modules
- Scripts must contain an exported default function - they will be considered a ENTRY POINT for the test

# Running
- Docker `docker run -i grafana/k6 run - <script.js`
- Linux/Max `k6 run script.js`
- Windows `k6.exe run script.js`