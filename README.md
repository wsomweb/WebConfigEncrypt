# WebConfigEncrypt

This program makes it easy to ensure that sensitive data in your IIS configuration files are kept secure. Recursively scans webroots to find all web.config files and ensure that sensitive regions are encrypted. Provides a simple GUI wrapper for `aspnet_regiis` commands. 

Given one or more webroot folders, the WebConfigEncrypt does the following:

* Performs a breadth-first traversal beginning in webroot(s) and skipping specified subfolders
	* Open web.config if present
	* Check for defined sections of interest
	* If present, see if section is encrypted
* Displays list of sections of interest and their encryption status
	* Includes a button to toggle encryption state
		* When decrypting, just call command
		* When encrypting, search the web.config for key providers
			* If only one provider present, use it; if multiple, prompt user for which one to use

## Getting Started

Run the `WebConfigEncrypt.exe` executable to launch the program. The program needs to be run on the surver running IIS and requires Administrator privileges in order to encrypt/decrypt sections.

### Managing RSA Key Containers

Before encrypting or decrypting any configuration settings, you must first have an RSA Key Container configured to work with IIS. WebConfigEncrypt wraps the `aspnet_regiis` commands for creating, exporting, importing, and configuring these keys. 

Click **Manage RSA Key Containers** in the main menu or on the configuration form to access these tools. For example, the steps below walk you through creating a new key container called "SharedKeys" and using it in a multi-server web farm:

1.	Run WebConfigEncrypt on one of the servers in the web farm to create and export the key container
	1.	Create the container
		1.	Click **Create RSA Key Container**
		2.	Enter "SharedKeys" as the key container name
	2.	Export the container
		1.	Click **Export RSA Key Container**
		2.	Select "SharedKeys" from the drop-down list of containers
		3.	Select where to save the key container (as an XML file)
	3.	Copy the exported XML somewhere that you'll be able to access it from the remaining servers
2.	Run WebConfigEncrypt on the remaining servers in the web farm to import the key container
	1.	Import the container
		1.	Click **Import RSA Key Container**
		2.	Select the XML file you generated in step 1.2.3
		3.	Enter "SharedKeys" in the text box
3.	Grant permission to the key container on _all servers_ in the web farm (including the server used to create it)
	1.	Click **Grant RSA Key Container Permissions**
	2.	Select "SharedKeys" from the drop-down list of containers
	3.	Enter the Windows user name to grant permissions to:
		*	IIS versions 7.5 and later use `APPPOOL\YourAppPoolName` as the default user for each application pool
		*	Previous IIS versions use `NT AUTHORITY\NETWORK SERVICE` as the default user for all application pools
4.	Add the key container to necessary web.config files (on one server in the farm)
	1.	Click **Add RSA Key Container to web.confg**
	2.	Select the web.config file to configure
	3.	Select "SharedKeys" from the drop-down list of containers
5.	Scan, encrypt, distribute
	1.	Follow the directions below to scan for and encrypt any sensitive configuration sections
	2.	Distribute the updated web.config files (with key container configuration and encrypted contents) to remaining servers in web farm
		*	Since the same key container was exported and imported across all the servers in the farm, any server will be able to decrypt values encrypted on any other server

### Configuration

#### ASP Version

By default, WebConfigEncrypt is configured to run with .NET 4.0. If you are using an older version of .NET, you will need to update the values of `AspNetRegIisPath` and `SystemConfigurationVersion` in `WebConfigEncrypt.exe.config`'s `appSettings` section to match your system.

#### Interactive

On the first run, you will be prompted to choose at least one folder to search within under **Webroots to Scan**. You can enter these manually (one per line), or you can load them from an `applicationHost.config` file. File system permissions usually do not allow the program to load the file in its default location, so you will likely need to make a copy of the file.

Under **Subdirectories to Skip**, you can define subdirectories that will not be scanned (one per line). This check is done on the last section of the filename (e.g. an entry of `_vti_cnf` will skip `C:\www\webroot\foo\_vti_cnf` and any child directories). 

The **Sections to Scan** section lets you define which sections to check are encrypted in `web.config` files. Each line should include an XPath selector to a configuration section.

#### WebConfigEncrypt.exe.config

Alternatively, you can configure these values in the `WebConfigEncrypt.exe.config` file's `appSettings` section. The **Webroots**, **ExcludeDirectories**, and **IncludeSections** keys take a comma-, semicolon-, or pipe-delimited list of values.

## Credits

WebConfigEncrypt was developed by [Kinsey Roberts](https://github.com/kinzdesign) for the [Weatherhead School of Management](https://weatherhead.case.edu/) at [Case Western Reserve University](https://case.edu/).

Inspiration and `aspnet_regiis` command examples came from [DarkThread](http://blog.darkthread.net/)'s [Web Config ConnectionString Encryptor](https://archive.codeplex.com/?p=webconfigenc).

[Windows CryptoAPI wrapper code](https://security.stackexchange.com/questions/1771/how-can-i-enumerate-all-the-saved-rsa-keys-in-the-microsoft-csp/102923#102923) adapted from [Derek W on StackExchange](https://security.stackexchange.com/users/43390/derek-w).

## License
WebConfigEncrypt is released under the [MIT License](LICENSE).
