<#
.SYNOPSIS
	Project Orchestration
.PARAMETER BuildImage
	Builds the docker image (compose).
.PARAMETER Clean
	Removes the image and kills all containers based on that image.
.PARAMETER Compose
	Builds and runs a Docker image.
.PARAMETER ComposeForDebug
    Builds the image and runs docker-compose.
.PARAMETER Setup
	Setup the project.
.PARAMETER Environment
	The environment to build for (Debug or Release), defaults to Debug.
.EXAMPLE
	C:\PS> .\project-tasks.ps1 -BuildImage -Environment debug 
#>

# #############################################################################
# PARAMS
#
[CmdletBinding(PositionalBinding = $false)]
Param(
    # Commands
    [Switch]$BuildImage,
    [Switch]$Clean,
    [Switch]$Compose,
    [Switch]$ComposeForDebug,
    # Flags
    [String]$Environment = "debug"
)


# #############################################################################
# GLOBAL VARIABLES
#
$Environment = $Environment.ToLowerInvariant()
$StartTime      = $(Get-Date)
$WorkingDir = (Get-Item -Path ".\" -Verbose).FullName


# #############################################################################
# COMMANDS
# #############################################################################

$ErrorActionPreference = "Stop"


# #############################################################################
# Runs docker-compose.
#
Function BuildImage () {

    Write-Host "++++++++++++++++++++++++++++++++++++++++++++++++" -ForegroundColor "Green"
    Write-Host "+ Building docker image                         " -ForegroundColor "Green"
    Write-Host "++++++++++++++++++++++++++++++++++++++++++++++++" -ForegroundColor "Green"

    $composeFileName = "docker-compose.yml"
    If ($Environment -ne "Debug") {
        $composeFileName = "docker-compose.$Environment.yml"
    }

    If (Test-Path $composeFileName) {

        Write-Host "Building the image ($Environment)." -ForegroundColor "Yellow"
        docker-compose -f "$composeFileName" build
    }
    Else {
        Write-Error -Message "$Environment is not a valid parameter. File '$dockerFileName' does not exist." -Category InvalidArgument
    }

    If ($LASTEXITCODE -ne 0){    
        Write-Error -Message "The command `docker compose` exited with $LastExitCode"
    }
}


# #############################################################################
# Kills and removes running container
#
Function Clean () {

    Write-Host "++++++++++++++++++++++++++++++++++++++++++++++++" -ForegroundColor "Green"
    Write-Host "+ Cleaning docker images and containers         " -ForegroundColor "Green"
    Write-Host "++++++++++++++++++++++++++++++++++++++++++++++++" -ForegroundColor "Green"

    $composeFileName = "docker-compose.yml"
    If ($Environment -ne "debug") {
        $composeFileName = "docker-compose.$Environment.yml"
    }

    If (Test-Path $composeFileName) {

        docker-compose -f "$composeFileName" down --rmi all

        $danglingImages = $(docker images -q --filter 'dangling=true')
        If (-not [String]::IsNullOrWhiteSpace($danglingImages)) {
            docker rmi -f $danglingImages
        }
        Write-Host "Removed docker images" -ForegroundColor "Yellow"
    }
    Else {
        Write-Error -Message "$Environment is not a valid parameter. File '$composeFileName' does not exist." -Category InvalidArgument
    }
}


# #############################################################################
# Runs docker-compose.
#
Function Compose () {

    Write-Host "++++++++++++++++++++++++++++++++++++++++++++++++" -ForegroundColor "Green"
    Write-Host "+ Composing docker images                       " -ForegroundColor "Green"
    Write-Host "++++++++++++++++++++++++++++++++++++++++++++++++" -ForegroundColor "Green"

    $composeFileName = "docker-compose.yml"
    If ($Environment -ne "debug") {
        $composeFileName = "docker-compose.$Environment.yml"
    }
    
    Write-Host "Creating the container" -ForegroundColor "Yellow"
    docker-compose -f $composeFileName kill
    docker-compose -f $composeFileName up -d

    If ($LASTEXITCODE -ne 0){    
        Write-Error -Message "The command `docker compose` exited with $LastExitCode"
        exit 1
    }
}



# #############################################################################
# Setup the project
#
Function Setup () {

    Write-Host "++++++++++++++++++++++++++++++++++++++++++++++++" -ForegroundColor "Green"
    Write-Host "+ Setting up project                            " -ForegroundColor "Green"
    Write-Host "++++++++++++++++++++++++++++++++++++++++++++++++" -ForegroundColor "Green"

    # Place project init code here (npm init, etc)

    Write-Host "Done" -ForegroundColor "Yellow"
}



# #############################################################################
# HELPER FUNCTIONS
# #############################################################################


# #############################################################################
# Welcome Message
#
Function Welcome () {

    Write-Host ""
    Write-Host "                    __              " -ForegroundColor "Blue"
    Write-Host "   _________ _____ / /_____  ____   " -ForegroundColor "Blue"
    Write-Host "  / ___/ __ ``/_  // __/ __ \/ __ \ " -ForegroundColor "Blue"
    Write-Host " / /__/ /_/ / / // /_/ /_/ / / / /  " -ForegroundColor "Blue"
    Write-Host " \___/\__,_/ /___\__/\____/_/ /_/   " -ForegroundColor "Blue"        
    Write-Host ""                    

}


# #############################################################################
# Finished
#
Function Finished () {

    $elapsedTime = $(get-date) - $StartTime
    $totalTime = "{0:HH:mm:ss}" -f ([datetime]$elapsedTime.Ticks)

    Write-Host ""
    Write-Host "++++++++++++++++++++++++++++++++++++++++++++++++" -ForegroundColor "Blue"
    Write-Host "+ Finished in $totalTime"
    Write-Host "++++++++++++++++++++++++++++++++++++++++++++++++" -ForegroundColor "Blue"
    Write-Host ""

}


# #############################################################################
# EXECUTE
# #############################################################################

Welcome

If ($BuildImage) {
    BuildImage
}
ElseIf ($Clean) {
    CleanAll
}
ElseIf ($Compose) {
    BuildImage   
    Compose
}
ElseIf ($ComposeForDebug) {
    $env:REMOTE_DEBUGGING = "enabled"
    BuildImage   
    Compose
}

Finished

# #############################################################################
