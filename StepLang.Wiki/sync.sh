#!/usr/bin/env bash

set -e

# switch to wiki directory
pushd _wiki

# set up git
git config user.name "$USER_NAME"
git config user.email "$USER_EMAIL"
git remote add wiki "https://$USER_NAME:$USER_TOKEN@github.com/$REPOSITORY_NAME.wiki.git"

# if there are no changes, just exit
echo "Checking for changes..."
if [[ -z $(git status -s) ]]; then
    echo "No changes to commit."
    exit 0
fi

# commit and push
echo "Committing and pushing..."
git add .
git commit --verbose -m "$COMMIT_MSG"
git push --all --force-with-lease --set-upstream wiki master --verbose

popd
