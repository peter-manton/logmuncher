LogMuncher
==========

LogMuncher runs as service that collects log from several popular proxy servers - including Squid and Varnish (planned.)

Logs are stored within a MySQL database and can the service can then generate usage reports from this informationL

* Top User Downloads
* Most Requested URL's
* 404's / 503's

It is currently in alpha stage, although supports UDP logging from Squid and can generate usage reports.

It is written in C#, although runs well under Mono (tested on Debian Wheezy / Mono 2.10.

If you feel like you could contribute or have any suggestions please drop a line.

Enjoy!
