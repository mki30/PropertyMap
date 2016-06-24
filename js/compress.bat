del  .\cmp\*.js
del PropertyMap.js
type PropertyList.js >> PropertyMap.js
type jquery.fancybox.pack.js >> PropertyMap.js
type master.js >> PropertyMap.js
type common.js >> PropertyMap.js
type default.js >> PropertyMap.js

java -jar compiler.jar --js=PropertyMap.js --js_output_file=.\cmp\PropertyMap.js
del PropertyMap.js

