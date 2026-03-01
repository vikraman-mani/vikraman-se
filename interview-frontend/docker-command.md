### If you want to run the site using a docker file use the following

docker build -t rlfe:dev .
docker run -it --rm -v ${PWD}:/app -v /app/node_modules -p 3001:3001 -e CHOKIDAR_USEPOLLING=true rlfe:dev
