<?xml version='1.0' encoding='UTF-8' standalone='yes' ?>
<tagfile doxygen_version="1.17.0" doxygen_gitid="65a43c0aba45cc23b3ca11b6b5334d4eea931726">
  <compound kind="file">
    <name>mainpage.dox</name>
    <path>FIRE/docs/</path>
    <filename>d5/d4d/mainpage_8dox.html</filename>
  </compound>
  <compound kind="file">
    <name>Program.cs</name>
    <path>FIRE/FIRE.Console/</path>
    <filename>dd/d5c/_program_8cs.html</filename>
    <class kind="class">ProgramHost</class>
    <class kind="class">GenerateCommand</class>
    <class kind="class">InspectCommand</class>
    <class kind="class">CommonCommandSettings</class>
    <class kind="class">InspectSettings</class>
    <class kind="class">CommandExecutor</class>
    <class kind="class">ConsoleUi</class>
    <class kind="class">AppLifetime</class>
    <member kind="enumeration">
      <type></type>
      <name>UiLanguage</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a214a7ba4e60a1b01dc585e30bd48ba55</anchor>
      <arglist></arglist>
    </member>
    <member kind="enumvalue">
      <name>English</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a214a7ba4e60a1b01dc585e30bd48ba55a49bfa7fcc3d94cb262ab0af5f668b74d</anchor>
      <arglist></arglist>
    </member>
    <member kind="enumvalue">
      <name>German</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a214a7ba4e60a1b01dc585e30bd48ba55a2fcdfb5eb134d45c42707f9fd3def282</anchor>
      <arglist></arglist>
    </member>
    <member kind="enumvalue">
      <name>French</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a214a7ba4e60a1b01dc585e30bd48ba55afe66e5596e9202cd7ecff734de46cc09</anchor>
      <arglist></arglist>
    </member>
    <member kind="enumvalue">
      <name>Filipino</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a214a7ba4e60a1b01dc585e30bd48ba55aaa81505f396196fadf57bde2671bea1d</anchor>
      <arglist></arglist>
    </member>
    <member kind="function">
      <type>return ProgramHost</type>
      <name>Run</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>ab9b66f3c646f45e1ce7a086a3f4b22e0</anchor>
      <arglist>(args)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static internal class ProgramHost</type>
      <name>Execute</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>af7aa1c5f4ca117e7d07722000c6d1693</anchor>
      <arglist>(CommandContext context, CollectSettings settings)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static int</type>
      <name>Run</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>ada8ff3575ce87335d605fd36b0ce56fe</anchor>
      <arglist>(string[] args)</arglist>
    </member>
    <member kind="function">
      <type>GenerateCommand Command</type>
      <name>Execute</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a4d298343aa6fabe91377bf2383d0e39e</anchor>
      <arglist>(CommandContext context, CommonCommandSettings settings)</arglist>
    </member>
    <member kind="function">
      <type>InspectCommand Command</type>
      <name>Execute</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a05a6ef50979ce9024bf564fbcfe81e97</anchor>
      <arglist>(CommandContext context, DiagnoseSettings settings)</arglist>
    </member>
    <member kind="function">
      <type>override int</type>
      <name>Execute</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>adbf420779ad6d41f0b167fa8f6d5b3d3</anchor>
      <arglist>(CommandContext context, InspectSettings settings)</arglist>
    </member>
    <member kind="function">
      <type>CommonCommandSettings CommandSettings</type>
      <name>Description</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a4bcbd2f2d86ecbe814caecc9d758e6bb</anchor>
      <arglist>(&quot;Clear the database before collecting files.&quot;)][CommandOption(&quot;--clear-database|--clear&quot;)][DefaultValue(false)] public bool ClearDatabase</arglist>
    </member>
    <member kind="function">
      <type>InspectSettings CommonCommandSettings</type>
      <name>Description</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a6266d0ce0b1719c3afb0c0f0e64979a0</anchor>
      <arglist>(&quot;Path to the source file to diagnose.&quot;)][CommandOption(&quot;--source-path|-s &lt;SOURCE_PATH&gt;&quot;)] public string? SourcePath</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static internal class CommandExecutor</type>
      <name>RuntimeContext</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a3734b5c4cfa462c3c8ad9f8023893619</anchor>
      <arglist>(string CultureCode, UiLanguage Language, bool NoWrap)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static int</type>
      <name>ExecuteCollect</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a3346dc078fdb2e1bcdc66fd43b140f51</anchor>
      <arglist>(CollectSettings settings, RuntimeContext runtime)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static int</type>
      <name>ExecuteGenerate</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a463c764d5df5979b037b23c37a8ced77</anchor>
      <arglist>(CommonCommandSettings settings, RuntimeContext runtime)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static int</type>
      <name>ExecuteFileOperations</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a759d8d651d077248931371415a6e37c8</anchor>
      <arglist>(CommonCommandSettings settings, RuntimeContext runtime)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static int</type>
      <name>ExecuteInspect</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a7904950bbffd99fc2a1c3af59708a28f</anchor>
      <arglist>(InspectSettings settings, RuntimeContext runtime)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static int</type>
      <name>ExecuteDiagnose</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a088240f79721370912dd25fb7bf8501d</anchor>
      <arglist>(DiagnoseSettings settings, RuntimeContext runtime)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static string</type>
      <name>ReadCultureArgument</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a6a3aa44b6ef80017dd5069af551a9607</anchor>
      <arglist>(string[] args)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static string</type>
      <name>ResolveCultureCode</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>aab2d3182896fd5d30de14da4dc0d58e0</anchor>
      <arglist>(string rawCulture)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static UiLanguage</type>
      <name>ResolveLanguage</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>ab60fe8b2d858cd1a3bb8cfeae582dbe0</anchor>
      <arglist>(string cultureCode)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static string</type>
      <name>Get</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>abf42770faa2e6c7ebeb652c67d2f6395</anchor>
      <arglist>(UiLanguage language, string key)</arglist>
    </member>
    <member kind="function">
      <type>internal readonly record struct</type>
      <name>LocalizedText</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>ae7418d1d538743232e8a9bda612115d2</anchor>
      <arglist>(string English, string German, string French, string Filipino)</arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>ConfigPath</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a9decc718c0558cf9af1c9698998c812d</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>set</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a112a711f766db446c2f517b794a1c04a</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>Culture</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a0391b18642c9e16b7c2a285fc7db3786</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>bool</type>
      <name>NoWrap</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a5e22b3e09c3d1b18cc2a0dc70f3effe0</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>FilePath</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a65969f9f39cf0e1af9cea3b84b374c3c</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>OutputPath</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a6d029ed84fe1d5ba24ad30f0d5cb86c5</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>bool</type>
      <name>CopyPath</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a22cbcd22ebaebdb9a535d5c5437f54d3</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>internal enum UiLanguage</type>
      <name>string</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a4cb475094dc9de3af5eddaa1a7e87703</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>internal enum UiLanguage</type>
      <name>CultureAliases</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>af7c6e3ac502073155117e020ca5046f1</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable" static="yes">
      <type>static internal class ConsoleUi</type>
      <name>Texts</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a5621682479052f42bbe2d2046f5adf2c</anchor>
      <arglist></arglist>
    </member>
  </compound>
  <compound kind="file">
    <name>UnitTest1.cs</name>
    <path>FIRE/FIRE.Tests/</path>
    <filename>de/d6a/_unit_test1_8cs.html</filename>
    <class kind="class">UnitTest1</class>
  </compound>
  <compound kind="file">
    <name>FIRECatalog.cs</name>
    <path>FIRE/FIRE/</path>
    <filename>db/d24/_f_i_r_e_catalog_8cs.html</filename>
    <class kind="class">FileInfoMetadataSource</class>
    <class kind="class">MetadataSourceRegistry</class>
    <class kind="struct">FILE_ID_INFO</class>
    <member kind="enumeration">
      <type></type>
      <name>FileInfoByHandleClass</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>ae03b337fb935f5276a534f5220615928</anchor>
      <arglist></arglist>
    </member>
    <member kind="function">
      <type>enum FileInfoByHandleClass</type>
      <name>ExtractMetadata</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a9948088f14e009eff31758315993daa9</anchor>
      <arglist>(string filePath)</arglist>
    </member>
    <member kind="function">
      <type></type>
      <name>ExifToolMetadataSource</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a90486d21550d189c20fd6e8f38736057</anchor>
      <arglist>()</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>Dispose</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a6e2d745cdb7a7b983f861ed6a9a541a7</anchor>
      <arglist>()</arglist>
    </member>
    <member kind="function">
      <type>sealed class MetadataSourceRegistry</type>
      <name>DllImport</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a2a7ad26a66f0fc022a8788adfb0c262e</anchor>
      <arglist>(&quot;kernel32.dll&quot;, SetLastError=true)] private static extern bool GetFileInformationByHandle(SafeFileHandle hFile</arglist>
    </member>
    <member kind="function">
      <type></type>
      <name>MetadataSourceRegistry</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>ade8ca80f0c3053af5a3c46260b5b8a66</anchor>
      <arglist>()</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>Register</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>acabce2d08bc33eaeab6b941c1222b074</anchor>
      <arglist>(IMetadataSource source)</arglist>
    </member>
    <member kind="function">
      <type>IMetadataSource</type>
      <name>GetSource</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a24a60380fdba4ea641815f0200a92004</anchor>
      <arglist>(string sourceName)</arglist>
    </member>
    <member kind="function">
      <type></type>
      <name>FIRECatalog</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>aa1e8255547d501a2b9ed5236b92b1ff2</anchor>
      <arglist>(FIREConfigration configuration, FIREDatabase database)</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>CollectFiles</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a7699db9161ac26db6231f6b201d3b94b</anchor>
      <arglist>(Action&lt; string &gt;? progressCallback=null)</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>ClearDatabase</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a08c18d301e13f48def803bd50019d276</anchor>
      <arglist>()</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>GenerateTargetPaths</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>ade7e2647d98d1b16c3ab66b2f3beff0b</anchor>
      <arglist>(Action&lt; string &gt;? progressCallback=null)</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>ExecuteFileOperations</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a40541e840635f9fec2d30e5b2e85ca30</anchor>
      <arglist>(Action&lt; string &gt;? progressCallback=null)</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>LogCancelled</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a0e8ccea48e013f783535370544c3cfea</anchor>
      <arglist>()</arglist>
    </member>
    <member kind="function">
      <type>string</type>
      <name>DiagnoseGeneration</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>ac332f38150ec11ba50b9c3f637a3a346</anchor>
      <arglist>(string sourceFilePath)</arglist>
    </member>
    <member kind="function">
      <type>List&lt;(string Source, string Key, string Value)&gt;</type>
      <name>GetAllAvailableMetadata</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>aef7ba4394188e308379ddd8a487ac5f1</anchor>
      <arglist>(string filePath)</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>WriteMetadataToMarkdown</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>ab585df3741aa7104afc3c6b3d9b8e9e2</anchor>
      <arglist>(string filePath, string? outputPath=null)</arglist>
    </member>
    <member kind="variable">
      <type>enum FileInfoByHandleClass</type>
      <name>string</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a5d8d7c996845adbb001c9fe685ba97ca</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>FileBasicInfo</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>afea3e6f4e6095d3d0d5c3d1c3ceb4e6e</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>FileStandardInfo</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>ad04b748afd8c2c059087ab20fbacb276</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>FileNameInfo</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a0d02d911a08582556e386934a3f480df</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>FileRenameInfo</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a77180a79f688e4df4f2c4f9b7d4f73d2</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>FileDispositionInfo</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a7b513140296daaa82f40bb7938c3accc</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>FileAllocationInfo</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>aabad7ffbbc308dd17439b66f277ee2d0</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>FileEndOfFileInfo</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a38411d8f96c0de7b84546aeeb20bff8a</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>FileStreamInfo</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a244a54a21cb72b217b969b614dc50bfd</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>FileCompressionInfo</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>aaa451bcd7c50ba0d405334167166cd18</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>FileAttributeTagInfo</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a9f0f3b0a0b9a6c2e57fd02c9e8120a5b</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>FileIdBothDirectoryInfo</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a910aaf6f7ffe8acab14368ca6f3b4e47</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>FileIdBothDirectoryRestartInfo</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>ad5bc8dcae8aeb9b3c08f2e4ccdd73a84</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>FileIoPriorityHintInfo</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>ab1b69b8f32d8f8982103cf3ae5edd336</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>FileRemoteProtocolInfo</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a8acea0c8a6a4a95804b43508a52b4d14</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>FileFullDirectoryInfo</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a6ee330e8e1d029ffb0020c28ff265d66</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>FileFullDirectoryRestartInfo</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a9eaeba8c377cf28b78356bfcdf52148e</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>FileStorageInfo</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a00278eba084553a42cf32c32d033fe56</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>FileAlignmentInfo</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a31be267f5d78b2974b606c3bf4d559f3</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>FileIdInfo</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a2d7db4c1cdcd47ffbe1690c21841c4a4</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>FileIdExtdDirectoryInfo</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a62f1b547f9a527e902984c79486fc8c1</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>FileIdExtdDirectoryRestartInfo</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>ad1170fe11f743d09a5c0215fe58a0bf6</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>MaximumFileInfoByHandleClass</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a3b415b0ea154baa8bfa5b3b188ce1323</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>SourceName</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a651798f55a94e395df9f863283ef447c</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>FileInfoMetadataSource</type>
      <name>IMetadataSource</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a1f15bce6bdcbc5520b861dd7ca51d6c7</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>FileInfoMetadataSource</type>
      <name>_exifTool</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>aa55c13c28dd3ff33c1e3c528f7afb54d</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>sealed class MetadataSourceRegistry out BY_HANDLE_FILE_INFORMATION</type>
      <name>lpFileInformation</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a6663de26f833f169d0389caa8d9d7244</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>event EventHandler&lt; FIRECatalogProgressEventArgs &gt;</type>
      <name>ProgressChanged</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>aead28fb01c01b00a7bbbd1557ed455d7</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>CultureInfo</type>
      <name>Culture</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>ab248cbfbd011375a62bf31c640ea6559</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>set</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a112a711f766db446c2f517b794a1c04a</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>DateTime</type>
      <name>FallbackDateTime</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a18f9c0bac49bd7a5956079568a0ad53d</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>FIRECatalogStage</type>
      <name>CurrentStage</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>ad6e3ce861d4a5f5772ce00c59c91789a</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>CurrentFilePath</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a4a6c1207ff87a242380f60f9491e640e</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>int</type>
      <name>ProcessedFileCount</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a0c7afb1a46673160b6732204f4f48c93</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>int</type>
      <name>TotalFileCount</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>ab7bc51d4a61158d80661ad7805b8210b</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>IReadOnlyList&lt; string &gt;</type>
      <name>LastCollectedSourcePaths</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a7e4b773b2216d56b9b9d2fb9fae30d4f</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>struct FILE_ID_INFO</type>
      <name>FileAttributes</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a2c44d103de50fc202e076d05702be646</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>System Runtime InteropServices ComTypes FILETIME</type>
      <name>CreationTime</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a5b34ed4fd541dd9ebdbf8b7f03dd12d8</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>System Runtime InteropServices ComTypes FILETIME</type>
      <name>LastAccessTime</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a4a2cbe49aac2cf4a70e718c5d358b80b</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>System Runtime InteropServices ComTypes FILETIME</type>
      <name>LastWriteTime</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>adca2c4faa6f3b5005d34dcda1abba8cb</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>uint</type>
      <name>VolumeSerialNumber</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a8ab4d933b535f75cc90072052e631ab6</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>uint</type>
      <name>FileSizeHigh</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>ae5398e3ef19fee6587c5121f76e18110</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>uint</type>
      <name>FileSizeLow</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a9fa80beffdf0d5c8dd40408ede398e29</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>uint</type>
      <name>NumberOfLinks</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>acfa36f4ecafea68f8667cc0931ce2801</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>uint</type>
      <name>FileIndexHigh</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a4ed5a3b450a4ddd42233f787181de3dc</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>uint</type>
      <name>FileIndexLow</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>af020a49b57fe3e772eeec3aa6e466144</anchor>
      <arglist></arglist>
    </member>
  </compound>
  <compound kind="file">
    <name>FIRECatalogProgress.cs</name>
    <path>FIRE/FIRE/</path>
    <filename>dc/d87/_f_i_r_e_catalog_progress_8cs.html</filename>
    <member kind="enumeration">
      <type></type>
      <name>FIRECatalogStage</name>
      <anchorfile>dc/d87/_f_i_r_e_catalog_progress_8cs.html</anchorfile>
      <anchor>a47c4de6beb40683c67f85cbdebf55b59</anchor>
      <arglist></arglist>
    </member>
    <member kind="enumvalue">
      <name>Collect</name>
      <anchorfile>dc/d87/_f_i_r_e_catalog_progress_8cs.html</anchorfile>
      <anchor>a47c4de6beb40683c67f85cbdebf55b59a65dddcfa19b099a3493bd593dbfd2b92</anchor>
      <arglist></arglist>
    </member>
    <member kind="enumvalue">
      <name>Generate</name>
      <anchorfile>dc/d87/_f_i_r_e_catalog_progress_8cs.html</anchorfile>
      <anchor>a47c4de6beb40683c67f85cbdebf55b59a13f619682461c16618971bf40437b4ef</anchor>
      <arglist></arglist>
    </member>
    <member kind="enumvalue">
      <name>Execute</name>
      <anchorfile>dc/d87/_f_i_r_e_catalog_progress_8cs.html</anchorfile>
      <anchor>a47c4de6beb40683c67f85cbdebf55b59a31b7313c05d32519f3869a3de8be95e6</anchor>
      <arglist></arglist>
    </member>
    <member kind="enumvalue">
      <name>Inspect</name>
      <anchorfile>dc/d87/_f_i_r_e_catalog_progress_8cs.html</anchorfile>
      <anchor>a47c4de6beb40683c67f85cbdebf55b59adcf3bba8dca8116c2b3a5c37c2ca1d16</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>enum FIRECatalogStage</type>
      <name>Trace</name>
      <anchorfile>dc/d87/_f_i_r_e_catalog_progress_8cs.html</anchorfile>
      <anchor>a6461b873ebed382efa7e574fd3ddbfe2</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>enum FIRECatalogStage</type>
      <name>Info</name>
      <anchorfile>dc/d87/_f_i_r_e_catalog_progress_8cs.html</anchorfile>
      <anchor>a95b85e952064b6939b16e62cb9590e91</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>enum FIRECatalogStage</type>
      <name>Warning</name>
      <anchorfile>dc/d87/_f_i_r_e_catalog_progress_8cs.html</anchorfile>
      <anchor>a95e3544a64bdc9ce3c1a03c4ea6cc208</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>enum FIRECatalogStage</type>
      <name>get</name>
      <anchorfile>dc/d87/_f_i_r_e_catalog_progress_8cs.html</anchorfile>
      <anchor>ac79c4ab1dd17857b215662c294e93616</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>init</name>
      <anchorfile>dc/d87/_f_i_r_e_catalog_progress_8cs.html</anchorfile>
      <anchor>a978c1602c1c480e86c09f3c3a1b8cc98</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>required FIRECatalogMessageLevel</type>
      <name>Level</name>
      <anchorfile>dc/d87/_f_i_r_e_catalog_progress_8cs.html</anchorfile>
      <anchor>a705b4dedbaecef5be6dfc859c77c44b6</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>required string</type>
      <name>Message</name>
      <anchorfile>dc/d87/_f_i_r_e_catalog_progress_8cs.html</anchorfile>
      <anchor>afb283ab001bbf2518427dae6320efcfa</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>MessageKey</name>
      <anchorfile>dc/d87/_f_i_r_e_catalog_progress_8cs.html</anchorfile>
      <anchor>afe0fe171a4ab9aa2291d8919719ab6e9</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>CurrentFilePath</name>
      <anchorfile>dc/d87/_f_i_r_e_catalog_progress_8cs.html</anchorfile>
      <anchor>a4a6c1207ff87a242380f60f9491e640e</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>int</type>
      <name>ProcessedCount</name>
      <anchorfile>dc/d87/_f_i_r_e_catalog_progress_8cs.html</anchorfile>
      <anchor>a944c06e999e15c1b345fcc6b7720e747</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>int</type>
      <name>TotalCount</name>
      <anchorfile>dc/d87/_f_i_r_e_catalog_progress_8cs.html</anchorfile>
      <anchor>a9dd7dc8c4fec548749f9e6619d237885</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>CultureInfo</type>
      <name>Culture</name>
      <anchorfile>dc/d87/_f_i_r_e_catalog_progress_8cs.html</anchorfile>
      <anchor>ab248cbfbd011375a62bf31c640ea6559</anchor>
      <arglist></arglist>
    </member>
  </compound>
  <compound kind="file">
    <name>FIREConfigration.cs</name>
    <path>FIRE/FIRE/</path>
    <filename>d1/d3c/_f_i_r_e_configration_8cs.html</filename>
    <class kind="class">FIREConfigration</class>
    <class kind="class">AvailableKeywordConfiguration</class>
    <member kind="function">
      <type>sealed class FIREConfigration</type>
      <name>YamlMember</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>a3612a17b60733dfb291baab823502312</anchor>
      <arglist>(Alias=&quot;FileType&quot;)] public string FileType</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static FIREConfigration</type>
      <name>Load</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>a016bde7230cf88484e9dfc8af96bb1b4</anchor>
      <arglist>(string filePath)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static FIREConfigration</type>
      <name>Parse</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>a1f4427fec476e7a156258bbcd80116b0</anchor>
      <arglist>(string yaml)</arglist>
    </member>
    <member kind="function">
      <type>bool</type>
      <name>IsConfigurationVersionSupported</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>a2127e8e5c4beba369943506c78f33a56</anchor>
      <arglist>()</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>EnsureSupportedConfigurationVersion</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>a2909ce7c3cae497a7297eeef183e00d6</anchor>
      <arglist>()</arglist>
    </member>
    <member kind="function">
      <type>internal void</type>
      <name>Normalize</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>a1c35f2ed6f60246e742841a824a7cb75</anchor>
      <arglist>(string databasePath)</arglist>
    </member>
    <member kind="variable">
      <type>const decimal</type>
      <name>SupportedConfigurationVersion</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>aa5389a725ea3efe7c6a97c603bbabb12</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>decimal</type>
      <name>ConfigurationVersion</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>a4cc8b64f5a1600f0837112c910b82f41</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>set</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>a112a711f766db446c2f517b794a1c04a</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>List&lt; string &gt;</type>
      <name>FilesRootPath</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>ae5a75378c68884efcb0e1c484ae21c23</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>DataBasePath</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>a4e707e6802e2d6aff362f368045e4cc6</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>DataBaseFileName</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>a1c9cab30d3476465f61856edfc4e81c8</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>Action</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>a6cbcc18d725aac9ed8a4f9040221606c</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>MediaRootPath</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>a181818c385b63ee4fe297f873b3478a1</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>RootPath</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>a75b2b0dd23b0061dde23e70727d89fa1</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>SortingPatern</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>a3cbabcf9e16d587d8c46845ae16fbadb</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>FileNamePatern</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>a7b7e81bdf960f55fe8288e5549d94c38</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>Dictionary&lt; string, string &gt;</type>
      <name>StringReplacements</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>afdba3e756dc6f3cbf4d4d8303cdfb4ff</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>Dictionary&lt; string, FileExtensionConfiguration &gt;</type>
      <name>FileExtensions</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>a218756e7b9554057f91a6fbfa85803a9</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>LoggingConfiguration</type>
      <name>Logging</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>aa3a64aa45e67c53f8f3af3c5653cd729</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>List&lt; string &gt;</type>
      <name>FileSorting</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>ac1a440ebd9b6b228eb302c563d496d86</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>FileSortingOrder</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>ac60df9be0840e23a27e41eaca9f08a37</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>FileClass</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>a75f7ba10d4e93a8705107796240bc8ad</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>List&lt; string &gt;</type>
      <name>SidecarFileExtensions</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>a17fb8db34a5e91f9f3c5726747f32226</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>Dictionary&lt; string, AvailableKeywordConfiguration &gt;</type>
      <name>AvailableKeyWords</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>a63323b332ded7ed3a4edc12fc1754d16</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>DataType</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>ae0846a526671ac8ea5c31fb1ed328a9d</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>Source</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>ac34de2a54dcaa5e190990544213c2a65</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>ValAttribute</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>a386f09885d44e5915217eb752f47e561</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>Default</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>a57818ca0924f9791fd2e538c3e12ccd7</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>List&lt; string &gt;</type>
      <name>SourceFields</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>a37a875d99b6438db746b8d3696a10dc3</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>LogFileName</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>a65d8359dcb16e8d807b59a51d9bffce6</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>LogLevel</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>a9647e3ff64f75fba67077f0021234871</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>long</type>
      <name>MaxFileSizeBytes</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>a5a7cd280c18f28e71c9851170346ba89</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>int</type>
      <name>MaxAgeDays</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>a356140f47d350238184f6084793a0513</anchor>
      <arglist></arglist>
    </member>
  </compound>
  <compound kind="file">
    <name>FIREDatabase.cs</name>
    <path>FIRE/FIRE/</path>
    <filename>d7/d1a/_f_i_r_e_database_8cs.html</filename>
    <class kind="class">FIREFileMetaData</class>
    <member kind="variable">
      <type>class FIREFileMetaData</type>
      <name>RegularFile</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a686291d47e5e4f2cb120f8c76f559a03</anchor>
      <arglist></arglist>
    </member>
  </compound>
  <compound kind="file">
    <name>ApiLocalizer.cs</name>
    <path>FIRE/FIRE/Localization/</path>
    <filename>dd/d1c/_api_localizer_8cs.html</filename>
    <class kind="class">ApiLocalizer</class>
  </compound>
  <compound kind="file">
    <name>FIRELogger.cs</name>
    <path>FIRE/FIRE/Logging/</path>
    <filename>d1/dcc/_f_i_r_e_logger_8cs.html</filename>
    <member kind="enumeration">
      <type></type>
      <name>FIRELogLevel</name>
      <anchorfile>d1/dcc/_f_i_r_e_logger_8cs.html</anchorfile>
      <anchor>a98b250e2290324629b7467b158d9763d</anchor>
      <arglist></arglist>
    </member>
    <member kind="enumvalue">
      <name>Debug</name>
      <anchorfile>d1/dcc/_f_i_r_e_logger_8cs.html</anchorfile>
      <anchor>a98b250e2290324629b7467b158d9763dac909e86054cb6ad83c22bfc2b3e6e5b8</anchor>
      <arglist></arglist>
    </member>
    <member kind="enumvalue">
      <name>Info</name>
      <anchorfile>d1/dcc/_f_i_r_e_logger_8cs.html</anchorfile>
      <anchor>a98b250e2290324629b7467b158d9763da1cd805eaf0bb58a90fe7e7e4cf6a3cdc</anchor>
      <arglist></arglist>
    </member>
    <member kind="enumvalue">
      <name>Warning</name>
      <anchorfile>d1/dcc/_f_i_r_e_logger_8cs.html</anchorfile>
      <anchor>a98b250e2290324629b7467b158d9763da48f2bb70fceb692a2dedd8cea496c44b</anchor>
      <arglist></arglist>
    </member>
    <member kind="enumvalue">
      <name>Error</name>
      <anchorfile>d1/dcc/_f_i_r_e_logger_8cs.html</anchorfile>
      <anchor>a98b250e2290324629b7467b158d9763da4dfd42ec49d09d8c6555c218301cc30f</anchor>
      <arglist></arglist>
    </member>
    <member kind="enumvalue">
      <name>Critical</name>
      <anchorfile>d1/dcc/_f_i_r_e_logger_8cs.html</anchorfile>
      <anchor>a98b250e2290324629b7467b158d9763da6779101cd5bbfab8a7da94cccf1947e2</anchor>
      <arglist></arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>Dispose</name>
      <anchorfile>d1/dcc/_f_i_r_e_logger_8cs.html</anchorfile>
      <anchor>a6e2d745cdb7a7b983f861ed6a9a541a7</anchor>
      <arglist>()</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static internal FIRELogLevel</type>
      <name>ParseLevel</name>
      <anchorfile>d1/dcc/_f_i_r_e_logger_8cs.html</anchorfile>
      <anchor>a2b034749dc19768fcbb2f94957c4da2c</anchor>
      <arglist>(string levelName)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static internal FIRELogLevel</type>
      <name>FromMessageLevel</name>
      <anchorfile>d1/dcc/_f_i_r_e_logger_8cs.html</anchorfile>
      <anchor>a97a9719365174ef86f9d5e1a7f79c9f1</anchor>
      <arglist>(FIRECatalogMessageLevel level)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static internal string</type>
      <name>StageTag</name>
      <anchorfile>d1/dcc/_f_i_r_e_logger_8cs.html</anchorfile>
      <anchor>a7120565336b5ef0a841b6dac0d08b6b7</anchor>
      <arglist>(FIRECatalogStage? stage)</arglist>
    </member>
    <member kind="variable">
      <type>enum FIRELogLevel</type>
      <name>TableHeader</name>
      <anchorfile>d1/dcc/_f_i_r_e_logger_8cs.html</anchorfile>
      <anchor>aa9eb0bb9975ab9d382645b69280f89dd</anchor>
      <arglist></arglist>
    </member>
  </compound>
  <compound kind="file">
    <name>AssemblyInfo.cs</name>
    <path>FIRE/FIRE/Properties/</path>
    <filename>d7/d2f/_assembly_info_8cs.html</filename>
  </compound>
  <compound kind="file">
    <name>README.md</name>
    <path>FIRE/</path>
    <filename>d9/dd6/_r_e_a_d_m_e_8md.html</filename>
  </compound>
  <compound kind="file">
    <name>THIRD-PARTY-NOTICES.md</name>
    <path>FIRE/</path>
    <filename>da/dc3/_t_h_i_r_d-_p_a_r_t_y-_n_o_t_i_c_e_s_8md.html</filename>
  </compound>
  <compound kind="class">
    <name>ApiLocalizer</name>
    <filename>d6/d50/class_api_localizer.html</filename>
    <member kind="function" static="yes">
      <type>static string</type>
      <name>Get</name>
      <anchorfile>d6/d50/class_api_localizer.html</anchorfile>
      <anchor>ae20112b68f53db89f0334a53bbe80e3c</anchor>
      <arglist>(string key, CultureInfo? culture=null)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static string</type>
      <name>Format</name>
      <anchorfile>d6/d50/class_api_localizer.html</anchorfile>
      <anchor>ac202062c7eea1de336ba7e23144ddf5a</anchor>
      <arglist>(string key, CultureInfo? culture=null, params object[] args)</arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>AppLifetime</name>
    <filename>d5/d39/class_app_lifetime.html</filename>
    <member kind="function" static="yes">
      <type>static void</type>
      <name>ThrowIfCancellationRequested</name>
      <anchorfile>d5/d39/class_app_lifetime.html</anchorfile>
      <anchor>a369ef0f0b61b7a75d0a8f1c27c124922</anchor>
      <arglist>()</arglist>
    </member>
    <member kind="variable" static="yes">
      <type>static volatile bool</type>
      <name>IsCancellationRequested</name>
      <anchorfile>d5/d39/class_app_lifetime.html</anchorfile>
      <anchor>a4150c509f15565982db5d75b89d30c75</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable" static="yes">
      <type>static UiLanguage</type>
      <name>CurrentLanguage</name>
      <anchorfile>d5/d39/class_app_lifetime.html</anchorfile>
      <anchor>a625e8e0b1dd8fea33ab714ab65e14605</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable" static="yes">
      <type>static bool</type>
      <name>NoWrap</name>
      <anchorfile>d5/d39/class_app_lifetime.html</anchorfile>
      <anchor>aa4954b22aaffe20d10b1ec69791992bd</anchor>
      <arglist></arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>AvailableKeywordConfiguration</name>
    <filename>d3/daf/class_available_keyword_configuration.html</filename>
    <member kind="function">
      <type>internal void</type>
      <name>Normalize</name>
      <anchorfile>d3/daf/class_available_keyword_configuration.html</anchorfile>
      <anchor>aaaf826e5080a9390662f87633d38aac7</anchor>
      <arglist>()</arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>DataType</name>
      <anchorfile>d3/daf/class_available_keyword_configuration.html</anchorfile>
      <anchor>a644c6e36c655560123458574410f7bc2</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>set</name>
      <anchorfile>d3/daf/class_available_keyword_configuration.html</anchorfile>
      <anchor>a0498422cf2d9f165fc718b6ed09a3196</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>Source</name>
      <anchorfile>d3/daf/class_available_keyword_configuration.html</anchorfile>
      <anchor>a36bf41d9f6134301933ea084b1dd4b25</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>ValAttribute</name>
      <anchorfile>d3/daf/class_available_keyword_configuration.html</anchorfile>
      <anchor>ad6370667010a92c28493fcc16aef5936</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>Default</name>
      <anchorfile>d3/daf/class_available_keyword_configuration.html</anchorfile>
      <anchor>af691edf1cbae194bc942a15bef257b25</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>List&lt; string &gt;</type>
      <name>SourceFields</name>
      <anchorfile>d3/daf/class_available_keyword_configuration.html</anchorfile>
      <anchor>a3347583a2ee7a223d0da4f56c71d65cb</anchor>
      <arglist></arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>CommandExecutor</name>
    <filename>d7/d0d/class_command_executor.html</filename>
    <member kind="function" static="yes">
      <type>static int</type>
      <name>ExecuteCollect</name>
      <anchorfile>d7/d0d/class_command_executor.html</anchorfile>
      <anchor>a64f3a81ce8a66385fe4ed97c0f9e204f</anchor>
      <arglist>(CollectSettings settings, RuntimeContext runtime)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static int</type>
      <name>ExecuteGenerate</name>
      <anchorfile>d7/d0d/class_command_executor.html</anchorfile>
      <anchor>aef2bbef9c6774e4ae30d057fbce4e490</anchor>
      <arglist>(CommonCommandSettings settings, RuntimeContext runtime)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static int</type>
      <name>ExecuteFileOperations</name>
      <anchorfile>d7/d0d/class_command_executor.html</anchorfile>
      <anchor>a2d9930af82fd26a8946da417d5d0256a</anchor>
      <arglist>(CommonCommandSettings settings, RuntimeContext runtime)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static int</type>
      <name>ExecuteInspect</name>
      <anchorfile>d7/d0d/class_command_executor.html</anchorfile>
      <anchor>a757146be08c3b7cd35e0efd9365c7115</anchor>
      <arglist>(InspectSettings settings, RuntimeContext runtime)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static int</type>
      <name>ExecuteDiagnose</name>
      <anchorfile>d7/d0d/class_command_executor.html</anchorfile>
      <anchor>abc8cb3b374f80ad8f8b7a33c3419b15c</anchor>
      <arglist>(DiagnoseSettings settings, RuntimeContext runtime)</arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>CommonCommandSettings</name>
    <filename>d0/d9e/class_common_command_settings.html</filename>
    <member kind="variable">
      <type>string</type>
      <name>ConfigPath</name>
      <anchorfile>d0/d9e/class_common_command_settings.html</anchorfile>
      <anchor>aaa15837d79523f89c9d99c7a44f4f259</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>set</name>
      <anchorfile>d0/d9e/class_common_command_settings.html</anchorfile>
      <anchor>aa0bffefed288afb85538d0be682c1bb8</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>Culture</name>
      <anchorfile>d0/d9e/class_common_command_settings.html</anchorfile>
      <anchor>a12d17daa3c10244483a02d32e37ec909</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>bool</type>
      <name>NoWrap</name>
      <anchorfile>d0/d9e/class_common_command_settings.html</anchorfile>
      <anchor>aa4cb9afc1b59297b0d422e85b22ac564</anchor>
      <arglist></arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>ConsoleUi</name>
    <filename>d2/d24/class_console_ui.html</filename>
    <member kind="function" static="yes">
      <type>static void</type>
      <name>WriteTitle</name>
      <anchorfile>d2/d24/class_console_ui.html</anchorfile>
      <anchor>a18c94d956c0ef084155a77b910f303ce</anchor>
      <arglist>(RuntimeContext runtime, string text)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static void</type>
      <name>WriteLine</name>
      <anchorfile>d2/d24/class_console_ui.html</anchorfile>
      <anchor>aaccf20c274cb719c400a88ecf7dc9780</anchor>
      <arglist>(RuntimeContext runtime, string text)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static void</type>
      <name>WriteInfo</name>
      <anchorfile>d2/d24/class_console_ui.html</anchorfile>
      <anchor>a9ab6f321aa3c0dfb04a997d2ff17ff38</anchor>
      <arglist>(RuntimeContext runtime, string text)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static void</type>
      <name>WriteSuccess</name>
      <anchorfile>d2/d24/class_console_ui.html</anchorfile>
      <anchor>abee1e21de2ad03307ff483bf464631a2</anchor>
      <arglist>(RuntimeContext runtime, string text)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static void</type>
      <name>WriteWarning</name>
      <anchorfile>d2/d24/class_console_ui.html</anchorfile>
      <anchor>a64c9cd2e532be51f5db212173191a1f5</anchor>
      <arglist>(RuntimeContext runtime, string text)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static void</type>
      <name>WriteWarning</name>
      <anchorfile>d2/d24/class_console_ui.html</anchorfile>
      <anchor>a6fb0291675e61c943ab65e5322a69818</anchor>
      <arglist>(UiLanguage language, bool noWrap, string text)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static void</type>
      <name>WriteError</name>
      <anchorfile>d2/d24/class_console_ui.html</anchorfile>
      <anchor>a4bb34e565c34f2f3db3effcef81e4eba</anchor>
      <arglist>(RuntimeContext runtime, string text)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static void</type>
      <name>WriteError</name>
      <anchorfile>d2/d24/class_console_ui.html</anchorfile>
      <anchor>aa8ca2ec3eb2f6de8e146b6a5d22ed3c7</anchor>
      <arglist>(UiLanguage language, bool noWrap, string text)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static void</type>
      <name>WriteProgress</name>
      <anchorfile>d2/d24/class_console_ui.html</anchorfile>
      <anchor>a6aacf2a49e00506ea525fb297790364d</anchor>
      <arglist>(RuntimeContext runtime, string text)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static void</type>
      <name>EndProgressLine</name>
      <anchorfile>d2/d24/class_console_ui.html</anchorfile>
      <anchor>abc2699c431798a542a9264b5a2f74180</anchor>
      <arglist>()</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static void</type>
      <name>WriteEmptyLine</name>
      <anchorfile>d2/d24/class_console_ui.html</anchorfile>
      <anchor>a692482b3b022aed6cd622fb35c4b995c</anchor>
      <arglist>()</arglist>
    </member>
  </compound>
  <compound kind="struct">
    <name>FILE_ID_INFO</name>
    <filename>db/d24/_f_i_r_e_catalog_8cs.html</filename>
    <anchor>d0/d27/struct_f_i_l_e___i_d___i_n_f_o</anchor>
    <member kind="variable">
      <type>ulong</type>
      <name>VolumeSerialNumber</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a564cffd9bc32c2920de981959cba0e99</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>byte[]</type>
      <name>FileId</name>
      <anchorfile>db/d24/_f_i_r_e_catalog_8cs.html</anchorfile>
      <anchor>a711a5d502adb83a88a94bf22b23c01fa</anchor>
      <arglist></arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>FileInfoMetadataSource</name>
    <filename>d3/df3/class_file_info_metadata_source.html</filename>
    <member kind="function">
      <type>Dictionary&lt; string, string &gt;</type>
      <name>ExtractMetadata</name>
      <anchorfile>d3/df3/class_file_info_metadata_source.html</anchorfile>
      <anchor>a1be4fd752b1fa3dea7bc6960b594e9af</anchor>
      <arglist>(string filePath)</arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>SourceName</name>
      <anchorfile>d3/df3/class_file_info_metadata_source.html</anchorfile>
      <anchor>a32cba1600aaabce5aaba6ca1d8a189a7</anchor>
      <arglist></arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>FIREConfigration</name>
    <filename>df/dbb/class_f_i_r_e_configration.html</filename>
    <member kind="function">
      <type>bool</type>
      <name>IsConfigurationVersionSupported</name>
      <anchorfile>df/dbb/class_f_i_r_e_configration.html</anchorfile>
      <anchor>a7a2fd301bd6cd3e6702d0d38d283c164</anchor>
      <arglist>()</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>EnsureSupportedConfigurationVersion</name>
      <anchorfile>df/dbb/class_f_i_r_e_configration.html</anchorfile>
      <anchor>a8c74ce8d2e821802ad7a0c5957826737</anchor>
      <arglist>()</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static FIREConfigration</type>
      <name>Load</name>
      <anchorfile>df/dbb/class_f_i_r_e_configration.html</anchorfile>
      <anchor>a89e6c0a8550de0522eda231500c9aa75</anchor>
      <arglist>(string filePath)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static FIREConfigration</type>
      <name>Parse</name>
      <anchorfile>df/dbb/class_f_i_r_e_configration.html</anchorfile>
      <anchor>ae50d1eadb52fb969c82846934f1d05f0</anchor>
      <arglist>(string yaml)</arglist>
    </member>
    <member kind="variable">
      <type>const decimal</type>
      <name>SupportedConfigurationVersion</name>
      <anchorfile>df/dbb/class_f_i_r_e_configration.html</anchorfile>
      <anchor>a87dd9cfaa69127725d9cb1b69edb28e6</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>decimal</type>
      <name>ConfigurationVersion</name>
      <anchorfile>df/dbb/class_f_i_r_e_configration.html</anchorfile>
      <anchor>afb4343218366cfc95236f616a3b9defb</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>set</name>
      <anchorfile>df/dbb/class_f_i_r_e_configration.html</anchorfile>
      <anchor>a2611d5688d84cdfd922318ad9f3fa451</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>List&lt; string &gt;</type>
      <name>FilesRootPath</name>
      <anchorfile>df/dbb/class_f_i_r_e_configration.html</anchorfile>
      <anchor>a7330a44bb06214211b69095e34b9df80</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>DataBasePath</name>
      <anchorfile>df/dbb/class_f_i_r_e_configration.html</anchorfile>
      <anchor>a9e21d63a7be7eb2fc97ce51348039180</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>DataBaseFileName</name>
      <anchorfile>df/dbb/class_f_i_r_e_configration.html</anchorfile>
      <anchor>a63707090391f44c2652c22d7394ad6e8</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>Action</name>
      <anchorfile>df/dbb/class_f_i_r_e_configration.html</anchorfile>
      <anchor>a215e3857068b982954a2c266f208a4af</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>MediaRootPath</name>
      <anchorfile>df/dbb/class_f_i_r_e_configration.html</anchorfile>
      <anchor>aa773e66f6dd7f501b1d4c5017ef23a2b</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>RootPath</name>
      <anchorfile>df/dbb/class_f_i_r_e_configration.html</anchorfile>
      <anchor>ae8b786d3e935425d04a7c35f24b2aa32</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>SortingPatern</name>
      <anchorfile>df/dbb/class_f_i_r_e_configration.html</anchorfile>
      <anchor>af2ad6573c54a4d718873d92d181a1553</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>FileNamePatern</name>
      <anchorfile>df/dbb/class_f_i_r_e_configration.html</anchorfile>
      <anchor>aae0421b0feb127698f270828b6f58881</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>Dictionary&lt; string, string &gt;</type>
      <name>StringReplacements</name>
      <anchorfile>df/dbb/class_f_i_r_e_configration.html</anchorfile>
      <anchor>a4c6ec16e124123c544bead8e64fb94dc</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>Dictionary&lt; string, FileExtensionConfiguration &gt;</type>
      <name>FileExtensions</name>
      <anchorfile>df/dbb/class_f_i_r_e_configration.html</anchorfile>
      <anchor>a076a2da18160edf92386073c50428b3e</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>LoggingConfiguration</type>
      <name>Logging</name>
      <anchorfile>df/dbb/class_f_i_r_e_configration.html</anchorfile>
      <anchor>a30ea6ef4d4731fb420cfe95dc06d4b5b</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>List&lt; string &gt;</type>
      <name>FileSorting</name>
      <anchorfile>df/dbb/class_f_i_r_e_configration.html</anchorfile>
      <anchor>a0c86337e613a78fea57b9e0eff64399c</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>FileSortingOrder</name>
      <anchorfile>df/dbb/class_f_i_r_e_configration.html</anchorfile>
      <anchor>a1879ddb0e28a602725563187cd86c8f6</anchor>
      <arglist></arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>FIREFileMetaData</name>
    <filename>d7/d1a/_f_i_r_e_database_8cs.html</filename>
    <anchor>d5/d85/class_f_i_r_e_file_meta_data</anchor>
    <member kind="variable">
      <type>long</type>
      <name>Id</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a22e6dd2966b87b8a4092886882b23cb6</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>set</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a2b4f3c6856d4b3b8393f5afc352854d7</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>Key</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>ae1619607ec188c07a67e190cccadd33a</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>Value</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a3015cff76ecc7428ca605b9d2908601b</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>TypeName</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a2f7c9c3aa885d033c6523e6715dba680</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>DataSource</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a5e8dbe87fbcfd5a8ec27a753ea1e2c83</anchor>
      <arglist></arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>GenerateCommand</name>
    <filename>d1/ddd/class_generate_command.html</filename>
    <member kind="function">
      <type>override int</type>
      <name>Execute</name>
      <anchorfile>d1/ddd/class_generate_command.html</anchorfile>
      <anchor>a1e40882f71a772183b444c4bb2843fdf</anchor>
      <arglist>(CommandContext context, CommonCommandSettings settings)</arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>InspectCommand</name>
    <filename>d1/d1a/class_inspect_command.html</filename>
    <member kind="function">
      <type>override int</type>
      <name>Execute</name>
      <anchorfile>d1/d1a/class_inspect_command.html</anchorfile>
      <anchor>a25ca614795f0cd12eb97cc6a5b8cc4f2</anchor>
      <arglist>(CommandContext context, InspectSettings settings)</arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>InspectSettings</name>
    <filename>dc/d79/class_inspect_settings.html</filename>
    <base protection="private">CommonCommandSettings</base>
    <member kind="variable">
      <type>string</type>
      <name>FilePath</name>
      <anchorfile>dc/d79/class_inspect_settings.html</anchorfile>
      <anchor>a6209ffeb1ccef782524b5d9833c86afc</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>set</name>
      <anchorfile>dc/d79/class_inspect_settings.html</anchorfile>
      <anchor>a1a11c20acc1fad915c310f07cd740702</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>OutputPath</name>
      <anchorfile>dc/d79/class_inspect_settings.html</anchorfile>
      <anchor>a13c9849cb24e7c2afab9353a61a08b5a</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>bool</type>
      <name>CopyPath</name>
      <anchorfile>dc/d79/class_inspect_settings.html</anchorfile>
      <anchor>aa934d169afcc140c2ab4ef13263542e4</anchor>
      <arglist></arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>MetadataSourceRegistry</name>
    <filename>de/df7/class_metadata_source_registry.html</filename>
    <member kind="function">
      <type></type>
      <name>MetadataSourceRegistry</name>
      <anchorfile>de/df7/class_metadata_source_registry.html</anchorfile>
      <anchor>a25cfb658e60966e8701aca954547f1a5</anchor>
      <arglist>()</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>Register</name>
      <anchorfile>de/df7/class_metadata_source_registry.html</anchorfile>
      <anchor>a818cb1a08d04816751b07875dca70ca7</anchor>
      <arglist>(IMetadataSource source)</arglist>
    </member>
    <member kind="function">
      <type>IMetadataSource</type>
      <name>GetSource</name>
      <anchorfile>de/df7/class_metadata_source_registry.html</anchorfile>
      <anchor>a7ddddd954540d49036c4a794e1923cd6</anchor>
      <arglist>(string sourceName)</arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>ProgramHost</name>
    <filename>d6/d5e/class_program_host.html</filename>
    <member kind="function" static="yes">
      <type>static int</type>
      <name>Run</name>
      <anchorfile>d6/d5e/class_program_host.html</anchorfile>
      <anchor>a44c8b8479de1a25cb20723db74971c45</anchor>
      <arglist>(string[] args)</arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>UnitTest1</name>
    <filename>d7/d81/class_unit_test1.html</filename>
    <member kind="function">
      <type>void</type>
      <name>ApplyStringReplacement_ExactPattern_ReplacesOnlyMatchingSubstring</name>
      <anchorfile>d7/d81/class_unit_test1.html</anchorfile>
      <anchor>a8c0a139372d19c57119440ae0c99ac6e</anchor>
      <arglist>()</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>ApplyStringReplacement_WildcardPattern_ReplacesMatchedSegment</name>
      <anchorfile>d7/d81/class_unit_test1.html</anchorfile>
      <anchor>a74b6b968b781d953e5f42f8c9bc67f48</anchor>
      <arglist>()</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>ApplyStringReplacement_RegexPrefix_UsesRegexReplacement</name>
      <anchorfile>d7/d81/class_unit_test1.html</anchorfile>
      <anchor>a18d7f604f48ef17c445760f13583eb90</anchor>
      <arglist>()</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>ResolveKeywordDefaultValue_DatetimeNow_UsesProvidedNowValueAndNormalizes</name>
      <anchorfile>d7/d81/class_unit_test1.html</anchorfile>
      <anchor>acbdf3495a3632ab888240892a7bf8e20</anchor>
      <arglist>()</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>ResolveKeywordDefaultValue_DatetimeString_NormalizesToDatabaseFormat</name>
      <anchorfile>d7/d81/class_unit_test1.html</anchorfile>
      <anchor>a440b4f806bae4271886f382d37f03bf6</anchor>
      <arglist>()</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>ResolveKeywordDefaultValue_InvalidDatetimeDefault_ReturnsNA</name>
      <anchorfile>d7/d81/class_unit_test1.html</anchorfile>
      <anchor>a65e3fed4c292dd081267784aaa6e5003</anchor>
      <arglist>()</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>ResolveKeywordDefaultValue_StringDefault_ReturnsDefaultUnchanged</name>
      <anchorfile>d7/d81/class_unit_test1.html</anchorfile>
      <anchor>a6c0ac62dfd6510e50f40cb54d35a8022</anchor>
      <arglist>()</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>ParseTemplate_DatetimeKeywordSupport_ResolvesNamedAndFormatSuffixes</name>
      <anchorfile>d7/d81/class_unit_test1.html</anchorfile>
      <anchor>a7e7977fedc94e152c72bd08af46b6e59</anchor>
      <arglist>()</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>ParseTemplate_NonDatetimeKeywordWithSuffix_DoesNotApplyDateFallback</name>
      <anchorfile>d7/d81/class_unit_test1.html</anchorfile>
      <anchor>a6d97b664e77b0b21fc6481dc99783fdd</anchor>
      <arglist>()</arglist>
    </member>
  </compound>
  <compound kind="page">
    <name>md__f_i_r_e_2_t_h_i_r_d-_p_a_r_t_y-_n_o_t_i_c_e_s</name>
    <title>Third-Party Notices</title>
    <filename>d3/d87/md__f_i_r_e_2_t_h_i_r_d-_p_a_r_t_y-_n_o_t_i_c_e_s.html</filename>
  </compound>
  <compound kind="dir">
    <name>FIRE/docs</name>
    <path>FIRE/docs/</path>
    <filename>dir_580823591837e4cab4ee5a22f2fff719.html</filename>
  </compound>
  <compound kind="dir">
    <name>FIRE</name>
    <path>FIRE/</path>
    <filename>dir_1f9768e2c593c5218470a8c4a49bbb18.html</filename>
    <dir>FIRE/docs</dir>
    <dir>FIRE/FIRE</dir>
    <dir>FIRE/FIRE.Console</dir>
    <dir>FIRE/FIRE.Tests</dir>
  </compound>
  <compound kind="dir">
    <name>FIRE/FIRE</name>
    <path>FIRE/FIRE/</path>
    <filename>dir_027ae1c642818204f1d2b0e289285598.html</filename>
    <dir>FIRE/FIRE/Localization</dir>
    <dir>FIRE/FIRE/Logging</dir>
    <dir>FIRE/FIRE/Properties</dir>
    <file>FIRECatalog.cs</file>
    <file>FIRECatalogProgress.cs</file>
    <file>FIREConfigration.cs</file>
    <file>FIREDatabase.cs</file>
  </compound>
  <compound kind="dir">
    <name>FIRE/FIRE.Console</name>
    <path>FIRE/FIRE.Console/</path>
    <filename>dir_34712068e75e4f8a851a4fa1ab20db3f.html</filename>
    <file>Program.cs</file>
  </compound>
  <compound kind="dir">
    <name>FIRE/FIRE.Tests</name>
    <path>FIRE/FIRE.Tests/</path>
    <filename>dir_168a0deea1a80b54accc11d218723f4f.html</filename>
    <file>UnitTest1.cs</file>
  </compound>
  <compound kind="dir">
    <name>FIRE/FIRE/Localization</name>
    <path>FIRE/FIRE/Localization/</path>
    <filename>dir_d924e2942acae4adf859cdb0d3f51e3c.html</filename>
    <file>ApiLocalizer.cs</file>
  </compound>
  <compound kind="dir">
    <name>FIRE/FIRE/Logging</name>
    <path>FIRE/FIRE/Logging/</path>
    <filename>dir_ecd01bc9629781be892d9e6de41ad66e.html</filename>
    <file>FIRELogger.cs</file>
  </compound>
  <compound kind="dir">
    <name>FIRE/FIRE/Properties</name>
    <path>FIRE/FIRE/Properties/</path>
    <filename>dir_25b85a9469b7ff762775243dc31c6afb.html</filename>
    <file>AssemblyInfo.cs</file>
  </compound>
  <compound kind="page">
    <name>index</name>
    <title>FIRE — File Information Reorganizer and Extractor</title>
    <filename>index.html</filename>
    <docanchor file="index.html" title="Overview">intro_sec</docanchor>
    <docanchor file="index.html" title="Architecture">arch_sec</docanchor>
    <docanchor file="index.html" title="API Localization">localization_sec</docanchor>
    <docanchor file="index.html" title="Three-Step Workflow">workflow_sec</docanchor>
    <docanchor file="index.html" title="Step 1 — collect">step1</docanchor>
    <docanchor file="index.html" title="Step 2 — generate">step2</docanchor>
    <docanchor file="index.html" title="Step 3 — execute">step3</docanchor>
    <docanchor file="index.html" title="Optional metadata inspection (&lt;tt&gt;inspect&lt;/tt&gt;)">inspect_step</docanchor>
    <docanchor file="index.html" title="CLI Culture and Output Options">cli_options_sec</docanchor>
    <docanchor file="index.html" title="Configuration">config_sec</docanchor>
    <docanchor file="index.html" title="Keyword Selection">keywords_sec</docanchor>
    <docanchor file="index.html" title="Metadata Sources">meta_sources_sec</docanchor>
    <docanchor file="index.html" title="Template Placeholders">placeholders_sec</docanchor>
    <docanchor file="index.html" title="Metadata Inspection Helpers">metadata_inspection_sec</docanchor>
    <docanchor file="index.html" title="GetAllAvailableMetadata">get_all_metadata_subsec</docanchor>
    <docanchor file="index.html" title="WriteMetadataToMarkdown">write_metadata_md_subsec</docanchor>
    <docanchor file="index.html" title="Sidecar Files">sidecar_sec</docanchor>
    <docanchor file="index.html" title="Configuration">sidecar_config</docanchor>
    <docanchor file="index.html" title="Collection Phase">sidecar_collection</docanchor>
    <docanchor file="index.html" title="Path Generation Phase">sidecar_generation</docanchor>
    <docanchor file="index.html" title="Execution Phase">sidecar_execution</docanchor>
    <docanchor file="index.html" title="License">license_sec</docanchor>
    <docanchor file="index.html" title="Third-Party Credits">credits_sec</docanchor>
    <docanchor file="index.html" title="Links">links_sec</docanchor>
  </compound>
</tagfile>
