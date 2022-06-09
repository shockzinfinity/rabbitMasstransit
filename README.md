# rabbitMasstransit

## rabbitmq docker-compose

```yaml
version: "3.4"

services:
  rabbitmq:
    image: rabbitmq:3-management-alpine
    container_name: rabbitmq
    restart: "no"
    ports:
      - "5672:5672"
      - "15672:15672"
```

## locust test

```bash
$ pip install locust
$ locust --version
```

```python
from locust import HttpUser, task, between

class User(HttpUser):
  # @task
  # def predict_iris(self):
  #     wait_time = between(1, 2)

  #     self.client.post("/CatalogItems", json={
  #         'name': 'locustTest',
  #         'description': 'Lorem ipsum dolor sit amet consectetur, adipisicing elit. Veritatis architecto tempore nulla magni, aspernatur esse dolor nisi blanditiis aliquid error vero illum? Fugit iusto quidem veritatis error fuga. Accusamus, et?',
  #         'price': 1230.00,
  #         'availableStock': 55,
  #         'maxStockThreshold': 23
  #     })

  @task
  def predict_iris(self):
    wait_time = between(1, 2)
    self.client.post("/OrderItems", json={
      "orderId": 99,
      "productId": 12,
      "productName": "string",
      "unitPrice": 55,
      "units": 3
    })

  def on_start(self):
    self.client.verify = False
    print("START LOCUST")

  def on_stop(self):
    print("STOP LOCUST")
```

```bash
$ locust -f locusttest.py
```
