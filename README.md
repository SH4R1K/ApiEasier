# ApiEasier

ApiEasier — это инструмент, который позволяет разработчикам и тестировщикам эмулировать поведение API для тестирования интеграций. Он особенно полезен в тех случаях, когда необходимо протестировать взаимодействие с внешними API, которые могут быть недоступны, нестабильны или требуют значительных затрат на использование.

## Команда разработчиков

- [@SH4R1K](https://github.com/SH4R1K) 
- [@Pluhenciya](https://github.com/Pluhenciya) 
- [@Meresk](https://github.com/Meresk) 
- [@rndviespl](https://github.com/rndviespl) 

## Стек технологий

### Бэкэнд
![C#](https://img.shields.io/badge/C%23-purple?style=for-the-badge&logo=dotnet&logoColor=white) ![ASP.NET](https://img.shields.io/badge/ASPNET-blueviolet?style=for-the-badge&logo=dotnet&logoColor=white) ![NLog](https://img.shields.io/badge/NLog-DA6821?style=for-the-badge&logo=dotnet&logoColor=white) ![MongoDB](https://img.shields.io/badge/MongoDB-brightgreen?style=for-the-badge&logo=mongodb&logoColor=white)

### Фронтэнд
![Angular](https://img.shields.io/badge/Angular-red?style=for-the-badge&logo=angular&logoColor=white) ![TaigaUI](https://img.shields.io/badge/TaigaUI-green?style=for-the-badge&logo=angular&logoColor=white) ![HTML](https://img.shields.io/badge/HTML-E34F26?style=for-the-badge&logo=html5&logoColor=white) ![CSS](https://img.shields.io/badge/CSS-1572B6?style=for-the-badge&logo=css3&logoColor=white) ![TypeScript](https://img.shields.io/badge/TypeScript-blue?style=for-the-badge&logo=typescript&logoColor=black)

### Другое

![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white) ![Nginx](https://img.shields.io/badge/Nginx-009639?style=for-the-badge&logo=nginx&logoColor=white)

## Установка

Для установки и запуска ApiEasier вам потребуется сервер с установленным Docker. Следуйте приведенным ниже шагам:

1. **Клонируйте репозиторий**:
   Откройте терминал и выполните команду для клонирования репозитория ApiEasier:
   ```bash
   git clone https://github.com/SH4R1K/ApiEasier.git
   ```

2. **Перейдите в директорию с проектом**:
   После клонирования репозитория перейдите в созданную директорию:
   ```bash
   cd ApiEasier
   ```

3. **Настройте переменные окружения**:
   Вам необходимо настроить переменные окружения для корректной работы приложения. Для этого скопируйте или отредактируйте файлы с переменными окружения:
   ```bash
   cp ./environment/default.env_example ./environment/default.env
   cp ./environment/mongodb.env_example ./environment/mongodb.env
   ```

4. **Запустите контейнеры**:
   После настройки переменных окружения выполните команду для запуска контейнеров с помощью Docker Compose:
   ```bash
   docker compose up -d
   ```
   Флаг `-d` запускает контейнеры в фоновом режиме.

### Проверка установки

После выполнения всех шагов вы можете проверить, что ApiEasier работает, открыв браузер и перейдя по адресу сервера. По умолчанию `http://serverip:5280`

### Остановка и удаление контейнеров

Если вам нужно остановить и удалить контейнеры, выполните следующую команду:
```bash
docker compose down
```

## Список контроллеров
| Название контроллера      | Назначение                                                                              |
| ------------------------- | --------------------------------------------------------------------------------------- |
| **ApiServiceController**  | Конфигурирование эмулируемых API сервисов                                               |
| **ApiEntityController**   | Конфигурирование сущностей эмулируемых API сервисов                                     |
| **ApiEndpointController** | Конфигурирование эндпоинтов внутри сущностей                                            |
| **ApiEmuController**      | Предоставляет возможность работы с эмулируемыми сервисами. Используется при интеграциях |
