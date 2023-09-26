module.exports = function (grunt) {
    require('load-grunt-tasks')(grunt);

    grunt.initConfig({
        clean: {
            build: ["wwwroot/dist/"],
            develop: ["wwwroot/dev/*", "!wwwroot/dev/mockServiceWorker.js"],
        },
        shell: {
            build: {
                command: 'npm run build',
            },
            develop: {
                command: 'npm run start',
            }
        }
    });

    grunt.loadNpmTasks("grunt-contrib-clean");
    grunt.loadNpmTasks('grunt-shell');

    grunt.registerTask("build", ['clean:build', 'shell:build']);
    grunt.registerTask("dev", ['clean:develop', 'shell:develop']);
};