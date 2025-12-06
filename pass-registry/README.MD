https://www.freecodecamp.org/news/how-to-self-host-a-container-registry/#heading-why-you-might-want-to-self-host-a-container-registry

mkdir -p ./registry/data

# install htpasswd

sudo apt install apache2-utils

# create a password file. username: busy, password: bee

htpasswd -Bbn busy bee > ./registry/registry.password
