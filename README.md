# MeiliSearch Demo

This repository provides a quick demonstration of how to set up MeiliSearch on your local machine using Docker and Make. 

MeiliSearch is an open-source search engine that offers fast and relevant full-text search capabilities for your data.

## Prerequisites

Before you get started, make sure you have the following prerequisites installed on your system:

- **Docker**: For containerization, you'll need Docker to run MeiliSearch. You can install it on both Windows and Linux.

  - [Install Docker on Windows](https://docs.docker.com/desktop/install/windows-install/)
  - [Install Docker on Linux](https://docs.docker.com/desktop/install/linux-install/)

- **Make**: You can use Make to simplify common tasks. Install it on both Windows and Linux.

  - [Install Make on Windows](http://gnuwin32.sourceforge.net/packages/make.htm)
  - [Install Make on Linux](https://www.gnu.org/software/make/)

## Getting Started

Clone this repository to your local machine:
```bash
git clone https://github.com/yourusername/meilisearch-demo.git
cd meilisearch-demo
```


To start MeiliSearch, run the following command:

```bash
make
```

This creates the required docker images locally

If you want to start MeiliSearch along with a companion service (e.g., a front-end app), use the following command:

```bash
make compose
```

This runs a docker-compose-stack with:
- MeiliSearch container
- Backend container
- Frontend container

Open your web browser and navigate to http://localhost:80.
You should see the MeiliSearch demo, and you can begin searching for movies.

## Additional Resources
[MeiliSearch Documentation](https://www.meilisearch.com/docs)
