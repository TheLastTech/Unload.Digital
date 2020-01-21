cd /home/bloodorange/MuhBot/;

git fetch;
LOCAL=$(git rev-parse HEAD);
REMOTE=$(git rev-parse @{u});

#if our local revision id doesn't match the remote, we will need to pull the changes
if [ $LOCAL != $REMOTE ]; then
    #pull and merge changes
    git pull origin master;

    #build the new site, you must install jekyll on the server, alternatively you could put the built _site
    #repo under version control and just update based off the changes in that folder. Jekyll outputs build into /_site
    yarn install && yarn run build && ./run.sh



    #change back to home directory 
    
    
    systemctl restart sleepy
    
  
fi