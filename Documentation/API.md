POST /customers
GET /customers
PUT /customers
GET /customers/{id}
DELETE /customers/{id}
GET /customers/{id}/tickets
GET /customers/{id}/bookings

POST /authors
GET /authors
PUT /authors
GET /authors/{id}
DELETE /authors/{id}
GET /authors/{id}/posters

POST /genres
GET /genres
PUT /genres
GET /genres/{id}
DELETE /genres/{id}

POST /posters
GET /posters
PUT /posters
GET /posters/{id}
DELETE /posters/{id}
GET /posters/search
GET /posters/{id}/tickets
GET /posters/{id}/available-tickets
GET /posters/{id}/stats

POST /ticket-infos
GET /ticket-infos
PUT /ticket-infos/{id}
GET /ticket-infos/{id}
DELETE /ticket-infos/{id}

POST /tickets/purchase
POST /tickets/book
POST /tickets/{bookingId}/confirm
GET /tickets/{id}
GET /tickets/validate/{ticketCode}
GET /tickets/available-seats/{posterId}
PUT /tickets/{id}/cancel

POST /bookings
GET /bookings
GET /bookings/{id}
GET /bookings/token/{token}
PUT /bookings/{id}/cancel
GET /bookings/expired
POST /bookings/cleanup-expired

POST /transactions
GET /transactions
GET /transactions/{id}
GET /transactions/customer/{customerId}
GET /transactions/ticket/{ticketId}