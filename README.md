# WebConfigEncrypt

This program makes it easy to ensure that sensitive data in your IIS configuration files are kept secure. 

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

Build the project and run the `WebConfigEncrypt.exe` executable to launch the program. The program needs to be run on the surver running IIS and requires Administrator privileges in order to encrypt/decrypt sections.

### Configuration

#### Interactive

On the first run, you will be prompted to choose at least one folder to search within under **Webroots to Scan**. You can enter these manually (one per line), or you can load them from an `applicationHost.config` file. File system permissions usually do not allow the program to load the file in its default location, so you will likely need to make a copy of the file.

Under **Subdirectories to Skip**, you can define subdirectories that will not be scanned (one per line). This check is done on the last section of the filename (e.g. an entry of `_vti_cnf` will skip `C:\www\webroot\foo\_vti_cnf` and any child directories). 

The **Sections to Scan** section lets you define which sections to check are encrypted in `web.config` files. Each line should include an XPath selector to a configuration section.

#### WebConfigEncrypt.exe.config

Alternatively, you can configure these values in the `WebConfigEncrypt.exe.config` file's `appSettings` section. The **Webroots**, **ExcludeDirectories**, and **IncludeSections** keys take a comma-, semicolon-, or pipe-delimited list of values.

## Credits
WebConfigEncrypt was developed by [Kinsey Roberts](https://github.com/kinzdesign) for the [Weatherhead School of Management](https://weatherhead.case.edu/) at [Case Western Reserve University](https://case.edu/).

## License
WebConfigEncrypt is released under the [MIT License](LICENSE).
