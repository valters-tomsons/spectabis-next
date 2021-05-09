# Pipelines

This directory contains pipeline configuration files for Spectabis build server hosted in Azure DevOps.

## Files

### azure-pipelines-service.yml

This pipeline builds, packages, and runs unit tests for Spectabis API service.

### azure-pipelines.yml

This pipeline builds, tests, and packages `spectabis-next` desktop clients for all platforms.

### azure-pr-ci.yml

This pipeline builds and tests all projects but does not build any artifacts. This pipeline runs for external pull requests.