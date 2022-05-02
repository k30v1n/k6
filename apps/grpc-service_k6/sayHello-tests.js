import grpc from 'k6/net/grpc';
import { check, sleep } from 'k6';

const client = new grpc.Client();
client.load(['../grpc-service/Protos'], 'greet.proto');

export default () => {
  client.connect('localhost:5022', {
    plaintext: true // only for localhost as it does not support TLS/SSL
  });

  const name = 'Bert';
  const data = { name: name };
  const response = client.invoke('greet.Greeter/SayHello', data);

  check(response, {
    'status is OK': (r) => r && r.status === grpc.StatusOK
  });

  console.log(JSON.stringify(response.message));

  client.close();
  sleep(1);
};
