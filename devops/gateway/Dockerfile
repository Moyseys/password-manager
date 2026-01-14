# Use a imagem oficial do KrakenD
FROM devopsfaith/krakend:2.5

# Copie o arquivo de configuração para o container
COPY krakend.json /etc/krakend/krakend.json

# Exponha a porta padrão do KrakenD
EXPOSE 8080

# O comando de inicialização já está definido na imagem base
# Mas podemos explicitá-lo se necessário
CMD ["run", "-c", "/etc/krakend/krakend.json"]
