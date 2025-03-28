# Conway's Game of Life API

A production-ready .NET 8 implementation of Conway's Game of Life with persistent board states and Swagger documentation.

## Problem Description
Conway's Game of Life is a cellular automaton that simulates cell evolution on an infinite grid using four simple rules. This API implements:

- Persistent board state management
- Efficient computation of future generations
- Stabilization detection within configurable steps
- Crash-resistant state storage
- RESTful interface with Swagger documentation

## Features
- Create and evolve multiple game boards
- Compute arbitrary future states
- Detect stabilization patterns
- SQLite database persistence
- Optimistic concurrency control
- Production-ready error handling
- Containerized deployment

## Installation & Setup

### Docker

```bash
# Build the image
docker build -t conways-game-of-life-api -f Dockerfile .

# Run container with persistent storage
docker run --name conways-game-of-life-api -d -p 8080:8080 conways-game-of-life-api