# Functionality Sketch
1. Parse applicationHosts.config to find bindings and extract webroots
2. Use queue for breadth-first traversal beginning in webroots
	1. Open web.config if present
	2. Find sections of interest
	3. If present, see if section is encrypted
3. Display list of sections of interest and their encryption status
	1. Include button to toggle encryption state
	2. When decrypting, just call command
	3. When encrypting, search the web.config for key providers
		1. If one provider, use it; if multiple, prompt user
