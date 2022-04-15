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
        self.client.post("/api/OrderItem", json={
            "orderId": 2,
            "productId": 1,
            "productName": "product1",
            "unitPrice": 340,
            "units": 3
        })

    def on_start(self):
        self.client.verify = False
        print("START LOCUST")

    def on_stop(self):
        print("STOP LOCUST")
