#!/bin/bash

# Project Orchestration

# #############################################################################
# CONSTANTS
#
BLUE="\033[00;94m"
GREEN="\033[00;92m"
RED="\033[00;31m"
RESTORE="\033[0m"
YELLOW="\033[00;93m"
ROOT_DIR=$(pwd)


# #############################################################################
# COMMANDS
# #############################################################################


# #############################################################################
# Kills all running containers of an image
#
clean() {

    echo -e "${GREEN}"
    echo -e "++++++++++++++++++++++++++++++++++++++++++++++++"
    echo -e "+ Cleaning docker images                        "
    echo -e "++++++++++++++++++++++++++++++++++++++++++++++++"
    echo -e "${RESTORE}"

    if [[ -z $ENVIRONMENT ]]; then
        ENVIRONMENT="development"
    fi

    composeFileName="docker-compose.yml"
    if [[ $ENVIRONMENT != "" ]]; then
        composeFileName="docker-compose.$ENVIRONMENT.yml"
    fi

    if [[ ! -f $composeFileName ]]; then
        echo -e "${RED}Environment '$ENVIRONMENT' is not a valid parameter. File '$composeFileName' does not exist. ${RESTORE}\n"
    else
        docker-compose -f $composeFileName down --rmi all

        # Remove any dangling images (from previous builds)
        danglingImages=$(docker images -q --filter 'dangling=true')
        if [[ ! -z $danglingImages ]]; then
        docker rmi -f $danglingImages
        fi

        rtn=$?
        if [ "$rtn" != "0" ]; then
            echo -e "${RED}An error occurred${RESTORE}"
            exit $rtn
        fi

        echo -en "${YELLOW}Removed docker images${RESTORE}\n"
    fi
}


# #############################################################################
# Runs docker-compose
#
buildImage () {

    echo -e "${GREEN}"
    echo -e "++++++++++++++++++++++++++++++++++++++++++++++++"
    echo -e "+ Building docker image                         "
    echo -e "++++++++++++++++++++++++++++++++++++++++++++++++"
    echo -e "${RESTORE}"

    if [[ -z $ENVIRONMENT ]]; then
        ENVIRONMENT="development"
    fi

    composeFileName="docker-compose.yml"
    if [[ $ENVIRONMENT != "development" ]]; then
        composeFileName="docker-compose.$ENVIRONMENT.yml"
    fi

    if [[ ! -f $composeFileName ]]; then
        echo -e "${RED}Environment '$ENVIRONMENT' is not a valid parameter. File '$composeFileName' does not exist. ${RESTORE}\n"
    else
        echo -e "${YELLOW}Building the image...${RESTORE}\n"
        docker-compose -f $composeFileName build
    fi

    rtn=$?
    if [ "$rtn" != "0" ]; then
        echo -e "${RED}An error occurred${RESTORE}"
        exit $rtn
    fi
}


# #############################################################################
# Runs docker-compose
#
compose () {

    echo -e "${GREEN}"
    echo -e "++++++++++++++++++++++++++++++++++++++++++++++++"
    echo -e "+ Composing docker images                       "
    echo -e "++++++++++++++++++++++++++++++++++++++++++++++++"
    echo -e "${RESTORE}"

    buildImage

    composeFileName="docker-compose.yml"
    if [[ $ENVIRONMENT != "development" ]]; then
        composeFileName="docker-compose.$ENVIRONMENT.yml"
    fi
        
    docker-compose -f $composeFileName kill
    docker-compose -f $composeFileName up -d
}


# #############################################################################
# Shows the usage for the script
#
showUsage () {

    echo -e "${YELLOW}"
    echo -e "Usage:"
    echo -e "    project-tasks.sh [COMMAND]"
    echo -e ""
    echo -e "Commands:"
    echo -e "    buildImage: Builds the docker image."
    echo -e "    clean: Removes the images and kills all containers based on that image."
    echo -e "    compose: Runs docker-compose."
    echo -e "    composeForDebug: Builds the image and runs docker-compose."
    echo -e ""
    echo -e "Environments:"
    echo -e "    development: Default environment."
    echo -e ""
    echo -e "Example:"
    echo -e "    ./project-tasks.sh compose debug"
    echo -e ""
    echo -e "${RESTORE}"

}


# #############################################################################
# HELPER FUNCTIONS
# #############################################################################


# #############################################################################
# Welcome message
#
welcome () {
    
    echo -e "${BLUE}"
    echo -e "                    __              " 
    echo -e "   _________ _____ / /_____  ____   " 
    echo -e "  / ___/ __ \`/_  // __/ __ \/ __ \ " 
    echo -e " / /__/ /_/ / / // /_/ /_/ / / / /  " 
    echo -e " \___/\__,_/ /___\__/\____/_/ /_/   " 
    echo -e "${RESTORE}"
    
}


# #############################################################################
# EXECUTE
# #############################################################################

welcome

if [ $# -eq 0 ]; then
    showUsage
else
    ENVIRONMENT=$(echo -e $2 | tr "[:upper:]" "[:lower:]")

    case "$1" in
        "build-image")
            buildImage
            ;;        
        "clean")
            clean
            ;;
        "compose")
            buildImage
            compose
            ;;
        "composeForDebug")
            export REMOTE_DEBUGGING="enabled"
            buildImage
            compose
            ;;
        *)
            showUsage
            ;;
    esac
fi


# #############################################################################
