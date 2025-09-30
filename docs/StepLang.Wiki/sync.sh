#!/usr/bin/env bash

set -e

echo "Cloning wiki..."
git clone "https://github.com/$GITHUB_REPOSITORY.wiki.git" _wiki

echo "Syncing wiki files..."
rsync -av --delete docs/StepLang.Wiki/ _wiki/ --exclude .git --exclude StepLang.Wiki.csproj --exclude packages.lock.json

# switch to wiki directory
pushd _wiki

# set up git
git config user.name "$USER_NAME"
git config user.email "$USER_EMAIL"
git remote add wiki "https://$USER_NAME:$USER_TOKEN@github.com/$GITHUB_REPOSITORY.wiki.git"
git fetch wiki

# if there are no changes, just exit
echo "Checking for changes..."
if [[ -z $(git status -s) ]]; then
    echo "No changes to commit."
    exit 0
fi

echo "Committing and pushing..."
git add .
git commit -m "$COMMIT_MSG" --verbose
git push --force-with-lease --set-upstream wiki master --verbose

popd
