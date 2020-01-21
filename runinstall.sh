chmod +x PreInstallXv.sh 
chmod +x InstallNode.sh
./InstallNode.sh
./PreInstallXv.sh 
./PreInstallXv.sh all-deps
./PreInstallXv.sh 
./PreInstallXv.sh xvfb-install

sudo yum install -y gtk3 gtk3-devel
 sudo yum install GConf2-devel GConf2