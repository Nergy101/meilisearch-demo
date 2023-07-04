build-images:
	docker build -t meili-backend:v1 --file src/backend/Dockerfile src/backend
	docker build -t meili-frontend:v1 --file src/frontend/Dockerfile src/frontend 

commpose-d:
	docker-compose --env-file ./config/.env.dev -d up

compose:
	docker-compose --env-file ./config/.env.dev up

local-run-backend:
	cd src/backend/MeiliSearchDemo/MeiliSearchDemo && dotnet run --project MeiliSearchDemo.csproj

local-run-frontend:
	cd src/frontend && npm run start


clean-local-db:
	rm -rf data/

clean-local-db-win: 
	rmdir /s /q "data"
