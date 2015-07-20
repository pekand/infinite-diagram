#!/bin/bash
for var in "$@"
do
    if [ "$var" = "-x" ]; then
        set -x
    fi
done

if [ "$1" = "svn" ]; then

    if [ "$2" = "commit" ]; then
        echo -e "\e[31m Svn commit  \e[0m"

        MESSAGE=`date +%Y-%m-%d`
        if [ "$3" != "" ]; then
            MESSAGE=$3
        fi

        svn add --force .
        svn commit -m "$MESSAGE"
    fi

    if [ "$2" = "update" ]; then
        echo -e "\e[31m Svn update  \e[0m"

        svn update
    fi

    if [ "$2" = "status" ]; then
        echo -e "\e[31m Svn update  \e[0m"

        svn status
    fi

    if [ "$2" = "ignore" ]; then
        echo -e "\e[31m Svn set ignore list  \e[0m"
        svn propset svn:ignore -F ignorelist.txt .
    fi

    if [ "$2" = "" ]; then
        echo "svn commit {message}"
        echo "svn update"
        echo "svn status"
        echo "svn ignore - set ignore list"
    fi

elif [ "$1" = "build" ]; then
        cd ./install-linux/
        ./make-package.sh
        
elif [ "$1" = "install" ]; then
	cd ./install-linux/
        ./install-package.sh

elif [ "$1" = "clean" ]; then

    if [ "$2" = "project" ]; then
        echo -e "\e[31m Clean project  \e[0m"

        sudo chown kerberos:kerberos -R ./
        sudo find "./" -type d -exec chmod 775 {} \;
        sudo find "./" -type f -exec chmod 664 {} \;
        chmod 755 clean.sh
        chmod 755 svn-ignore.sh
        chmod 755 svn-submit.sh
        chmod 755 svn-ignore.sh
        chmod 755 svn-update.sh
        chmod 755 build-mono.sh
        chmod 755 ./install-linux/make-package.sh
        chmod 755 ./install-linux/install-package.sh
        rm -R ./Diagram.SRC/Diagram/bin/Debug/
        rm -R ./Diagram.SRC/Diagram/bin/Release/
        #rm ./install-linux/turbo-diagram.deb
    fi

    if [ "$2" = "" ]; then
        echo "clean"
        echo "  project"
    fi
else
    echo "svn"
    echo "build mono"
    echo "clean project"
fi
