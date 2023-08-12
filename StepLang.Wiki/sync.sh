#!/usr/bin/env bash

set -e

# set up git
git config user.name "$USER_NAME"
git config user.email "$USER_EMAIL"
git remote add wiki "https://$USER_NAME:$USER_TOKEN@github.com/$REPOSITORY_NAME.wiki.git"

# if there are no change, just exit
git status --porcelain && exit 0

git add .
git commit --verbose -m "$COMMIT_MSG"
git push -f --set-upstream wiki master --verbose
