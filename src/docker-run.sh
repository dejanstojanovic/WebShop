#Consul container
docker run -d --net=host consul:latest

#RabbitMq container
docker run -d --rm --net=host rabbitmq:3.7.17-management