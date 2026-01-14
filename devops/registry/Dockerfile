FROM registry:latest

EXPOSE 5000

VOLUME ["/var/lib/registry", "/auth", "/certs"]

HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
	CMD wget --spider -q http://127.0.0.1:5000/v2/ || exit 1
