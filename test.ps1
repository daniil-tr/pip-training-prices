#!/usr/bin/env pwsh

Set-StrictMode -Version latest
$ErrorActionPreference = "Stop"

$component = Get-Content -Path "component.json" | ConvertFrom-Json
$image="$($component.registry)/$($component.name):$($component.version)-test"

# Set environment variables
$env:IMAGE = $image

try {
    # Workaround to remove dangling images
    docker-compose -f ./docker/docker-compose.test.yml down

    docker-compose -f ./docker/docker-compose.test.yml up --abort-on-container-exit --exit-code-from test

    $unitTestsExitCode = $LastExitCode
} finally {
    # Workaround to remove dangling images
    docker-compose -f ./docker/docker-compose.test.yml down
}

exit $unitTestsExitCode