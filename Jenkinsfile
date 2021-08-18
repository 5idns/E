pipeline {
    agent any
    stages {
        stage('Change Assembly Version'){
            steps {
                changeAsmVer assemblyCompany: '', 
                assemblyCopyright: '', 
                assemblyCulture: '', 
                assemblyDescription: '', 
                assemblyFile: '**/Directory.Build.props',
                assemblyProduct: '', 
                assemblyTitle: '', 
                assemblyTrademark: '', 
                regexPattern: '<(\\w*)Version>(\\d+).(\\d+).(\\d+).(\\d+)</(\\w*)Version>', 
                replacementPattern: '<$1Version>$2.$3.$4.%s</$6Version>', 
                versionPattern: '${BUILD_NUMBER}'
            }
        }
        stage('Restore NuGet Packages'){
            steps {
                dotnetRestore project: 'E.sln', 
                showSdkInfo: true, 
                workDirectory: './src/'
            }
        }
        stage('Build') {
            steps {
                dotnetBuild configuration: 'Release', 
                project: 'E.sln', 
                showSdkInfo: true, 
                workDirectory: './src/'
            }
        }
        stage('Publish API') {
            steps {
                dotnetPublish configuration: 'EasyBot.Api',
                outputDirectory: '../Publish/EasyBot.Api',
                project: 'EasyBot.Api.csproj',
                workDirectory: './EasyBot.Api'
            }
        }
    }
}