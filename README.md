# DKBasic
 DKBasic Compiler - C# educational programming IDE with lexical/syntactic analysis, AST, interpreter, and debugger. Supports variables, loops, procedures, and I/O operations.  **العربية:** مترجم DKBasic - بيئة تطوير #C تعليمية تحتوي محلل لغوي ونحوي، شجرة تجريدية، منفذ ومصحح أخطاء. تدعم المتغيرات، الحلقات، الإجرائيات وعمليات الإدخال والإخراج.
# وصف مشروع مترجم لغة DKBasic - DKBasic Compiler Project

## 📝 الوصف بالعربية

### 🎯 نظرة عامة
مترجم **DKBasic** هو مشروع متكامل تم تطويره بواسطة **المهندسة ديما خطيب** لتنفيذ وتحليل لغة البرمجة DKBasic. يمثل المشروع بيئة تطوير متكاملة تحتوي على محرر نصوص، مترجم، ومصحح أخطاء.

### ✨ الميزات الرئيسية
- **تحليل لغوي ونحوي كامل**
- **بناء شجرة تجريدية (AST)**
- **تنفيذ مباشر للكود**
- **نافذة تصحيح أخطاء متكاملة**
- **واجهة مستخدم رسومية بديهية**
- **دعم لأوامر البرمجة الهيكلية**

### 🛠️ التعليمات المدعومة
1. تعليمات الإسناد (`:=`)
2. الشروط (`if/else`)
3. الحلقات (`while`, `for`)
4. الإجرائيات (`procedure`, `call`)
5. الإدخال والإخراج (`Read`, `write`, `writeln`)
6. تعليمة التبديل (`switch`)
7. عمليات الزيادة والنقصان (`++`, `--`)
8. تعليمات التحكم (`break`, `continue`)

### 🏗️ البنية التقنية
- **اللغة:** C#
- **الواجهة:** Windows Forms
- **الهيكل:** تحليل لغوي → تحليل نحوي → بناء AST → تنفيذ

### 📁 مكونات المشروع
 المجلد: Compiling_Steps (خطوات الترجمة)
يحتوي هذا المجلد على الملفات الأساسية الخاصة بمعالجة الكود:
📁 Execution_Tree (مجلد فرعي):
build_AST_Tree.cs شجرة التركيب المجردة
build_Execution_Tree.cs شجرة التنفيذ 
Execute.cs  interpreter و الذي يحاكي  عمل المفسر  DKBasic ملف التنفيذ للتعليمات البرمجية المكتوبة بلغة  
Free_Class.cs ملف لتحرير المساحات الذاكرية المحجوزة
Lexical_Analysis.cs  المحلل اللغوي أو المعجمي Lexer
Optimization.cs مرحلة الأمثلة أو التحسين
Syntax_Analysis.cs المحلل النحوي parser
📁 المجلد: Forms (الواجهات الرسومية)
يحتوي على النوافذ الخاصة بالبرنامج:
AboutDKBasicForm.cs (نافذة "حول البرنامج")
InputForm.cs نافذة لادخال القيم من المستخدم 
MainForm.cs (النافذة الرئيسية)
OutputForm.cs النافذة التي تحوي التنفيذ النهائي للبرنامج
📁 المجلد: helper (أدوات مساعدة)
Error.cs ملف الأخطاء لمعالجة الخطأ و نوعه
Global.cs ملف للمتحولات و الميثودات العامة
📁 المجلد: structure (هيكلية اللغة/الكائنات)
يبدو أنها تحتوي على تعريفات لأنواع الجمل البرمجية (Nodes):

---

## 📝 English Description

### 🎯 Overview
**DKBasic Compiler** is a comprehensive project developed by **Engineer Dima Khatib** for parsing and executing the DKBasic programming language. The project represents a complete integrated development environment including a text editor, compiler, and debugger.

### ✨ Key Features
- **Full lexical and syntactic analysis**
- **Abstract Syntax Tree (AST) construction**
- **Direct code execution**
- **Integrated debug window**
- **Intuitive graphical user interface**
- **Structured programming commands support**

### 🛠️ Supported Instructions
1. Assignment statements (`:=`)
2. Conditional statements (`if/else`)
3. Loop structures (`while`, `for`)
4. Procedures (`procedure`, `call`)
5. Input/Output operations (`Read`, `write`, `writeln`)
6. Switch statements (`switch`)
7. Increment/Decrement operations (`++`, `--`)
8. Control flow statements (`break`, `continue`)

### 🏗️ Technical Architecture
- **Language:** C#
- **GUI Framework:** Windows Forms
- **Workflow:** Lexical → Syntactic → AST → Execution

### 📁 Project Components
- `Lexical_Analysis.cs` - Lexical analyzer
- `build_AST_Tree.cs` - AST builder
- `Interpreter.cs` - Code interpreter
- `Debug_Window.cs` - Debug window
- `AboutDKBasicForm.cs` - Language information

---

## 🚀 كيفية التشغيل - How to Run

### متطلبات النظام - System Requirements
- **.NET Framework 4.5+**
- **Windows OS**
- **Visual Studio (للتطوير)**

### خطوات التشغيل - Running Steps
1. **قم بفتح المشروع في Visual Studio**
2. **قم ببناء الحل (Build Solution)**
3. **شغل التطبيق (Run)**

---

## 📸 لقطات من المشروع - Project Screenshots

### الواجهة الرئيسية - Main Interface
```
[محرر النصوص]    [نافذة التصحيح]
[أزرار التنفيذ]  [معلومات المتغيرات]
```

### مثال على الكود - Code Example
```basic
x := 10;
if (x > 5) then {
    writeln('x is greater than 5');
}
```

---

## 📄 الترخيص - License
هذا المشروع تم تطويره لأغراض تعليمية وأكاديمية.

This project is developed for educational and academic purposes.

---

## 👩‍💻 المطور - Developer
**المهندسة ديما خطيب**  
**Engineer Dima Khatib**

---

الهاشتاقات العربية:
#مترجم_DKBasic #لغة_برمجة #مشروع_مترجم #مفسر_كود
#تصميم_لغات_برمجة #هندسة_برمجيات #نظرية_لغات
#محلل_لغوي #محلل_نحوي #شجرة_تجريدية
#تنفيذ_كود #برمجة_تعليمية #بيئة_تطوير
#مبرمج #تطوير_لغة #تمثيل_كود
#تحليل_لغوي #تحليل_نحوي #ترجمة_كود

:
#DKBasic #Compiler #ProgrammingLanguage #CSharp
#CompilerDesign #CustomLanguage #SoftwareEngineering
#LexicalAnalysis #SyntaxAnalysis #AST
#Interpreter #CodeExecution #IDE
#Parser #Lexer #Tokenization
#CodeGeneration #SymbolTable
#EducationalProgramming #Coding
#LanguageImplementation #DotNet

الهاشتاقات المختلطة:
#DKBasic_مترجم #Compiler_مشروع
#Programming_برمجة #Coding_برمجة
#LanguageDesign_تصميم_لغات
#Code_كود #Development_تطوير

ترتيب الهاشتاقات حسب المجموعات:

المشروع واللغة:
#DKBasic #مترجم_DKBasic
#ProgrammingLanguage #لغة_برمجة

التقنيات والأدوات:
#Compiler #مترجم #Interpreter #مفسر_كود
#CSharp #DotNet #IDE #بيئة_تطوير

التحليل والمعالجة:
#LexicalAnalysis #محلل_لغوي
#SyntaxAnalysis #محلل_نحوي
#Parser #AST #شجرة_تجريدية

التصميم والنظرية:
#CompilerDesign #تصميم_مترجم
#SoftwareEngineering #هندسة_برمجيات
#LanguageImplementation #تطوير_لغة

التعليم والأهداف:
#EducationalProgramming #برمجة_تعليمية
#Coding #برمجة
```

---.
