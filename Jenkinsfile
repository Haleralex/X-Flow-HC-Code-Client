pipeline {
    agent any

    environment {
        UNITY_PATH = '/home/haler/Unity/Hub/Editor/6000.2.12f1/Editor/Unity'
        PROJECT_PATH = "/UnityJenkins"
        BUILD_METHOD_WINDOWS = 'BuildScript.BuildWindows'
        KEYSTORE_PASS = 'unityjenkins'
        KEY_ALIAS_PASS = 'unityjenkins'
        POD_PATH = 'home/linuxbrew/.linuxbrew/bin/pod'
    }

    parameters {
        string(name: 'BRANCH', defaultValue: 'master', description: 'Git branch to build')
        choice(name: 'BUILD_TARGET', choices: ['Windows'], description: 'Build platforms')
        booleanParam(name: 'CLEAN_BUILD', defaultValue: true, description: 'Clean build by deleting Library folder')
        booleanParam(name: 'DEVELOPMENT_BUILD', defaultValue: false, description: 'Toggle Development Build, Autoconnect Profiler.')
        string(name: 'SCRIPTING_DEFINE_SYMBOLS', defaultValue: '', description: 'Scripting defines symbols separated by commas')
    }

    options { timestamps() }

    stages {
        stage('Checkout Code') {
            steps {
                script {
                    def repoUrl = sh(
                        script: '''
                            git remote -v | head -n 1 | awk '{print $2}'
                        ''',
                        returnStdout: true
                    ).trim()

                    if (!repoUrl) {
                        error "Unable to detect Repository URL — please check SCM configuration."
                    }

                    env.GIT_PATH = repoUrl
                    echo "Detected Git remote: ${env.GIT_PATH}"
                }

                checkout([
                    $class: 'GitSCM',
                    branches: [[ name: "*/${params.BRANCH}" ]],
                    userRemoteConfigs: [[ url: "${env.GIT_PATH}" ]]
                ])
            }
        }

        stage('Clean Library (Force Reimport All)') {
            when { expression { params.CLEAN_BUILD } }
            steps {
                sh '''
                echo "🧹 Cleaning Library folder to force Reimport All..."
                rm -rf "${PROJECT_PATH}/Library"
                echo "✅ Library cleaned. Unity will reimport all assets."
                '''
            }
        }

        stage('Build Windows') {
            when { expression { params.BUILD_TARGET == 'Windows' || params.BUILD_TARGET == 'Both MacOS Windows' } }
            environment {
                DEVELOPMENT_BUILD = "${params.DEVELOPMENT_BUILD}"
                SCRIPTING_DEFINE_SYMBOLS = "${params.SCRIPTING_DEFINE_SYMBOLS}"
            }
            steps {
                sh '''
                echo "WORKSPACE = $WORKSPACE"
                echo "PROJECT_PATH = $PROJECT_PATH"
                ls -la "$PROJECT_PATH"
                echo "🔨 Starting Unity Windows build..."
                echo "⚙️ DEVELOPMENT_BUILD=${DEVELOPMENT_BUILD}"
                ${UNITY_PATH} -quit -batchmode -nographics -projectPath "${PROJECT_PATH}" -logfile 'unity_build_log_windows.txt' -executeMethod ${BUILD_METHOD_WINDOWS} -buildTarget win64
                '''
            }
        }

        stage('Archive Unity Windows Build Log') {
            when { expression { params.BUILD_TARGET == 'Windows' || params.BUILD_TARGET == 'Both MacOS Windows' } }
            steps {
                script {
                    def logPath = "unity_build_log_windows.txt"
                    archiveArtifacts artifacts: logPath, fingerprint: true, allowEmptyArchive: true
                }
            }
        }

        stage('Zip Windows Build') {
            when { expression { params.BUILD_TARGET == 'Windows' || params.BUILD_TARGET == 'Both MacOS Windows' } }
            environment {
                DEVELOPMENT_BUILD = "${params.DEVELOPMENT_BUILD}"
                SCRIPTING_DEFINE_SYMBOLS = "${params.SCRIPTING_DEFINE_SYMBOLS}"
            }
            steps {
                sh '''
                echo "📦 Zipping Windows build..."
                cd "${PROJECT_PATH}/Builds"
                
                PREFIX=""
                if [ "${DEVELOPMENT_BUILD}" = "true" ]; then
                  PREFIX="DEV_BUILD_"
                fi
                
                if [ -n "${SCRIPTING_DEFINE_SYMBOLS}" ]; then
                  # Sanitize SCRIPTING_DEFINE_SYMBOLS: replace commas/semicolons with underscore, remove special chars
                  SYMBOLS_PREFIX=$(echo "${SCRIPTING_DEFINE_SYMBOLS}" | sed 's/[,;]/_/g' | sed 's/[^a-zA-Z0-9_]/_/g' | sed 's/__*/_/g' | sed 's/^_//' | sed 's/_$//')
                  if [ -n "$SYMBOLS_PREFIX" ]; then
                    PREFIX="${SYMBOLS_PREFIX}_${PREFIX}"
                  fi
                fi
                
                ZIPNAME="${PREFIX}WindowsBuild.zip"
                zip -r "${ZIPNAME}" Windows
                echo "✅ Windows build zipped as ${ZIPNAME}."
                '''
            }
        }

        stage('Archive MacOS and Window ZIP') {
            when { expression { params.BUILD_TARGET == 'MacOS' || params.BUILD_TARGET == 'Windows' || params.BUILD_TARGET == 'Both MacOS Windows' } }
            steps {
                script {
                    def relativePath = PROJECT_PATH == env.WORKSPACE ? '' : PROJECT_PATH - "${env.WORKSPACE}/"
                    def path = relativePath ? "${relativePath}/Builds/*.zip" : "Builds/*.zip"
                    archiveArtifacts artifacts: path, fingerprint: true
                }
            }
        }

        stage('Cleanup ZIP files') {
            when { expression { params.BUILD_TARGET == 'MacOS' || params.BUILD_TARGET == 'Windows' || params.BUILD_TARGET == 'Both MacOS Windows' } }
            steps {
                sh '''
                echo "🧹 Cleaning up Zip files in ${PROJECT_PATH}/Builds/"
                rm -f "${PROJECT_PATH}/Builds/"*.zip
                echo "✅ ZIP files cleaned up."
                '''
            }
        }
    }

    post {
        success { echo '✅ Build completed successfully!' }
        failure { echo '❌ Build failed!' }
    }
}
