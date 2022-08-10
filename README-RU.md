[![Build Status](https://github.com/Maslinin/Consyzer/workflows/Build/badge.svg)](https://github.com/Maslinin/Consyzer/actions/workflows/build.yml) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Maslinin_Consyzer&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Maslinin_Consyzer) [![GitHub license](https://badgen.net/github/license/Maslinin/Consyzer)](https://github.com/Maslinin/Consyzer/blob/master/LICENSE)

# Описание
*Consyzer* - это анализатор консистентности CIL файлов, предназначенный для предотвращения проблем в системе, связанных с нарушением консистентностии развернутых модулей приложения.

## Для чего?
Допустим, в исходном коде некоего приложения есть вызовы к функциям, находящимся в сторонних библиотеках динамической компоновки (DLL). Такие вызовы описываются в метаданных модуля приложения.
Ключевой особенностью таких инструкций импорта является то, 
что вызываемая из DLL функция не компонуется с исходным кодом модуля приложения; 
вместо этого в метаданных модуля сохраняется информация об импортируемой функции и местоположение модуля в системе, из которой эта функция импортируется.
Такой подход позволяет обращаться к нужному компоненту во время выполнения приложения.           

Когда все модули DLL находятся там, где они и должны быть согласно метаданным, приложение работает корректно, не нарушая целостность и безопасности системы; 
но если хотя бы один из модулей будет остутствовать, приложение не только завершит свою работу аварийно, но и может привести к нарушению безопасности всей системы.       
Consyzer разработан специально для анализа и предотвращения подобных инцедентов.

## Преим Consyzer?
1. *Consyzer* является независимым от GUI и запускается прямо из *CLI*.
2. *Consyzer* может быть интегрирован в конвейер CI/CD.
3. *Consyzer* логгирует подробную информацию об анализе в консоль и файл одновременно.
4. *Consyzer* возвращает конкретный код анализа операционной системе, что позволяет индивидуально реагировать на тот или иной инцидент анализа.

## Какую информацию логгирует Consyzer об импортируемой функции?
Если *Consyzer* обнаружил импортированную функцию, он логгирует следующую информацию об этой функции:
1. Имя функции
2. Сигнатуру функции
3. Местоположении модуля DLL, 
4. Аргументы атрибута DllImport

## Коды возврата
```
0 - консистентность артефактов проекта не нарушена;
1 - одна или несколько DLL, используемая в артефактах проекта, находится на абсолютном или относительном пути;
2 - одна или несколько DLL, используемая в артефактах проекта, отсутствуют на пути, указанном в метаданных модуля.
```

# Зависимости проектов
1. **Consyzer**
   - Consyzer.AnalyzerEngine *[Project]*
     - System.Reflection.Metadata *[NuGet]*
2. **Consyzer.Tests**
   - *Consyzer*
   - Microsoft.NET.Test.Sdk *[NuGet]*
   - xunit *[NuGet]*
   - xunit.runner.visualstudio *[NuGet]*
   - coverlet.collector *[NuGet]*
3. **Consyzer.AnalyzerEngine.Tests**
   - Consyzer.AnalyzerEngine
   - Microsoft.NET.Test.Sdk *[NuGet]*
   - xunit *[NuGet]*
   - xunit.runner.visualstudio *[NuGet]*
   - coverlet.collector *[NuGet]*

# Запуск    
В параметры командной строки утилите передается два параметра: 
1. Директория, содержащая CIL-файлы для анализа;
2. Расширения файлов, подлежащих анализу.

Общий шаблон запуска *Consyzer* из CLI выглядит следующим образом:
```
pathToConsyzer pathsToFolderWithAnalysisFiles ExtensionsOfAnalysisFiles
```

Пример запуска *Consyzer* из CLI:

```
C:\Consyzer.exe C:\AnalysisFolder ".exe, .dll"
```

## Сканирование нескольких проектов
Вы можете использовать приведенный ниже сценарий *PowerShell* для сканирования выходных артефактов с помощью *Consyzer* для нескольких проектов сразу. 
Например, это может пригодиться при сканировании артефактов решения в *конвейере CI*.

```
$pathToConsyzer = 'C:\Consyzer.exe'; $fileExtensionsToAnalysis = '".exe, .dll"'; $solutionToAnalysis = 'C:\SolutionToAnalysis'; $buildConfiguration = 'Release'

cd $solutionToAnalysis

#Артефакты проекта практически всегда содержат DLL; поэтому, во избежание дублирования найденных путей с артефактами, мы ищем только те пути, которые содержат DLL.
#Если Вы твердо уверены в том, что Ваше приложение не содержит DLL, замените в следующей строке "*.dll" на "*.exe".
$paths = $( Get-ChildItem -Path . -Include "*.dll"  -Recurse -Force )

#Поиск артефактов для сканирования
$pathsToAnalysisFiles = New-Object System.Collections.Generic.List[System.IO.FileInfo]
foreach($file in $paths) {
  if($file -match "bin" -and "$buildConfiguration" -and $file -notmatch "runtimes") {
	$pathsToAnalysisFiles.Add($file)
  }
}

if($pathsToAnalysisFiles.length -eq 0) {
  Write-Host "Бинарные файлы для анализа не обнаружены."
  Exit 0
}

#Получение выходных директорий с артефактами проектов для анализа
$AnalysisFolders = New-Object System.Collections.Generic.List[System.String]
foreach($file in $pathsToAnalysisFiles) {
  $folder = $($file.DirectoryName)
  $AnalysisFolders.Add($folder)
}
$AnalysisFolders = $AnalysisFolders | Select-Object -Unique

#Сканирование проектов
$exitCode = -1
$AnalysisStatuses = New-Object System.Collections.Generic.List[System.String]
foreach($folder in $AnalysisFolders) {
  & $pathToConsyzer $folder $fileExtensionsToAnalysis
  Write-Host "Consyzer run exit code: " $LastExitCode `n

  if ( $LastExitCode -ge $exitcode ) {
	  $exitcode = $LastExitCode
  }
  
  #Опредение статуса сканирования
  $Event = "Успешно:($folder): Проблем консистентности решения не обнаружено."
  if ($LastExitCode -eq '-1') { $Event = "Ошибка|$folder-> Consyzer не смог проанализировать файлы, так как произошла внутренняя ошибка. Убедитесь, что аргументы были переданы правильно." }
  if ($LastExitCode -eq '1' ) { $Event = "Предупреждение|$folder->  Одно или несколько используемых в проекте DLL компонентов находится на абсолютном или относительном пути." }
  if ($LastExitCode -eq '2' ) { $Event = "Ошибка|$folder->  Одно или несколько используемых в проекте DLL компонентов не обнаружено на ожидаемом пути." }
  $AnalysisStatuses.Add($Event)
}

foreach($status in $AnalysisStatuses) { Write-Host $status }
Exit $exitCode
```