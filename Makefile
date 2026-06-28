COMPOSE_FILE := docker/docker-compose.yml
WEB_PORT ?= 5080

.PHONY: help up down down-v

help:
	@echo "Targets:"
	@echo "  make up              Chạy Docker (website + MySQL + seed)"
	@echo "  make down            Dừng Docker"
	@echo "  make down-v          Dừng và xóa dữ liệu DB"
	@echo "  make up WEB_PORT=8888  Đổi port khi 5080 bị chiếm"

up:
	WEB_PORT=$(WEB_PORT) docker compose -f $(COMPOSE_FILE) up --build

down:
	docker compose -f $(COMPOSE_FILE) down

down-v:
	docker compose -f $(COMPOSE_FILE) down -v
