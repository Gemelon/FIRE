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
    <member kind="function">
      <type></type>
      <name>if</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a84ef8ed5fe64f697374a7ac3e173ca77</anchor>
      <arglist>(args.Length==0)</arglist>
    </member>
    <member kind="function">
      <type></type>
      <name>for</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a3693342da52d760d0681c068d1bff044</anchor>
      <arglist>(int i=1;i&lt; args.Length;i++)</arglist>
    </member>
    <member kind="function">
      <type></type>
      <name>if</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a74cf3b529baa4b4f9ea52790b08afefc</anchor>
      <arglist>(string.IsNullOrWhiteSpace(configPath))</arglist>
    </member>
    <member kind="function">
      <type></type>
      <name>if</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a366dc1663908062d6d0b2abaf5b80407</anchor>
      <arglist>(string.IsNullOrWhiteSpace(culture))</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static int</type>
      <name>ShowHelp</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>aaaf450777f9e0f705510063bb48a64f4</anchor>
      <arglist>()</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static int</type>
      <name>ExecuteCollect</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a33a1022b76d465aae062457c32f41e22</anchor>
      <arglist>(string configPath, string culture)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static int</type>
      <name>ExecuteGenerate</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a212272b90b3d6056be7582d063026024</anchor>
      <arglist>(string configPath, string culture)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static int</type>
      <name>ExecuteOperations</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a8c1c626b2d8cfd29330f1324532aed55</anchor>
      <arglist>(string configPath, string culture)</arglist>
    </member>
    <member kind="function" static="yes">
      <type>static string</type>
      <name>TruncatePath</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>aa8d9c0fbba0c4b1fbd8fba934e1a6014</anchor>
      <arglist>(string path, int maxLength)</arglist>
    </member>
    <member kind="variable">
      <type>var</type>
      <name>command</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a108b51efc46c603df382f88d19f8451e</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>configPath</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>ad62bfc699867fc1751fe0a6159827b01</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>culture</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>aaca62e4b78ac038ade08c841b7587678</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>return command</type>
      <name>switch</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a24768e217b121708b9b8e266eb8a6aa5</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>return command</type>
      <name>generate</name>
      <anchorfile>dd/d5c/_program_8cs.html</anchorfile>
      <anchor>a2322e5d351b925c75e98348014dd10a4</anchor>
      <arglist></arglist>
    </member>
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
    <name>FIREConfigration.cs</name>
    <path>FIRE/FIRE/</path>
    <filename>d1/d3c/_f_i_r_e_configration_8cs.html</filename>
    <class kind="class">FIREConfigration</class>
    <class kind="class">AvailableKeywordConfiguration</class>
    <member kind="function">
      <type>sealed class FIREConfigration</type>
      <name>YamlMember</name>
      <anchorfile>d1/d3c/_f_i_r_e_configration_8cs.html</anchorfile>
      <anchor>a4ace28242cd80c2f1d0c5f9fd9336d79</anchor>
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
  </compound>
  <compound kind="file">
    <name>FIREDatabase.cs</name>
    <path>FIRE/FIRE/</path>
    <filename>d7/d1a/_f_i_r_e_database_8cs.html</filename>
    <class kind="class">FIREFileMetaData</class>
    <class kind="class">FIREStatusRecord</class>
    <class kind="class">FIREFileMetaDataEntity</class>
    <class kind="class">FIREDatabaseStatusEntity</class>
    <member kind="function">
      <type></type>
      <name>FIREDatabase</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a634ff0b7db64e0e369c7d6c4e1546ee0</anchor>
      <arglist>(string databaseFilePath, bool recreateIfExists=false)</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>Reload</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a189465410ad8d15c2fc047de3fec0421</anchor>
      <arglist>()</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>RecreateDatabase</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a86fa889edb061cda3b853538e883724f</anchor>
      <arglist>()</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>Save</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a5df140c1ee51aadf7fe88ce3fb9cac33</anchor>
      <arglist>()</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>Add</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a3df032f8048c7a8a271bc4543458106b</anchor>
      <arglist>(FIREDbRecord item)</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>Clear</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>aa71d36872f416feaa853788a7a7a7ef8</anchor>
      <arglist>()</arglist>
    </member>
    <member kind="function">
      <type>bool</type>
      <name>Contains</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a9d19477ba8076ffadde03d422d7e80d8</anchor>
      <arglist>(FIREDbRecord item)</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>CopyTo</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a129fd3c3a3ee486599f4d9c2dc45b131</anchor>
      <arglist>(FIREDbRecord[] array, int arrayIndex)</arglist>
    </member>
    <member kind="function">
      <type></type>
      <name>PersistCurrentState</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a79b940850ffa15d822cd269b5a3def66</anchor>
      <arglist>(&quot;Updated&quot;, true, null)</arglist>
    </member>
    <member kind="function">
      <type>int</type>
      <name>IndexOf</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a7a42534f81d33a5577de1ba67bab2b97</anchor>
      <arglist>(FIREDbRecord item)</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>Insert</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a85a9f621e009230addade70c4e899698</anchor>
      <arglist>(int index, FIREDbRecord item)</arglist>
    </member>
    <member kind="function">
      <type>bool</type>
      <name>Remove</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a21c474c7a2521a2a3e292eea216bcb60</anchor>
      <arglist>(FIREDbRecord item)</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>RemoveAt</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>ad7c925d24a4e07389d2331e0508618ea</anchor>
      <arglist>(int index)</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>Dispose</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a6e2d745cdb7a7b983f861ed6a9a541a7</anchor>
      <arglist>()</arglist>
    </member>
    <member kind="function">
      <type>IReadOnlyList&lt; FIREDbRecord &gt;</type>
      <name>AsReadOnlyList</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a5dd091dca4c5a79a0a1e24224be17395</anchor>
      <arglist>()</arglist>
    </member>
    <member kind="function">
      <type>IEnumerator&lt; FIREDbRecord &gt;</type>
      <name>GetEnumerator</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>aa85eaf6280a192e8b5b7f02982c6bcfd</anchor>
      <arglist>()</arglist>
    </member>
    <member kind="variable">
      <type>class FIREFileMetaData</type>
      <name>get</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a25561f3ff869e8114067195daeaa8e2d</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>set</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a112a711f766db446c2f517b794a1c04a</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>byte[]</type>
      <name>FileId</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>aa1d2e27212394c8c5ec775073aeb5979</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>SourceFilePath</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a411d4d2f4e1601ede6c530814eddac90</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>TargetFilePath</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>ac056107d24b6e034d1dfe9a31870486f</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>List&lt; FIREFileMetaData &gt;</type>
      <name>FileMetaDatas</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a8b80e6a329ae6d42560b62a7705c9dea</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>class FIREStatusRecord</type>
      <name>FIREDbRecord</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a2575a6b0ec78cc6e7792e72e9b43bc16</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>class FIREStatusRecord</type>
      <name>_databaseFilePath</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>aae2512fda26e4e52afa72ec20c6a8fe5</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>DatabaseFilePath</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a5048d7d74229d33e3a7930474570423f</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>FIREStatusRecord</type>
      <name>StatusRecord</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a7219e35ae74e8a963204849ebf1cb8b6</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>int</type>
      <name>Count</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>aad462966ed963f892117056de1eba502</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>bool</type>
      <name>IsReadOnly</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>ad1b02f19e753582b3c5f9ed71bb0318a</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>FIREDbRecord</type>
      <name>this</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a0822c096825bf5a7fe8fe417b867c7e4</anchor>
      <arglist>[int index]</arglist>
    </member>
    <member kind="variable">
      <type>ulong</type>
      <name>VolumeSerialNumber</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>aa94be69a6059744e5390c3f376a5f388</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>Status</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>afdaa27edb811d806bc72f1d53c7334cc</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>bool</type>
      <name>Valid</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>aa8a69799bf8690d840821ef7d92baec8</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>DateTime</type>
      <name>TimeStamp</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a931e980ba3ac85bd305e2842d6425ceb</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>ErrorMessage</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a4a0b9b632a14e17b26850b7c5ddb6096</anchor>
      <arglist></arglist>
    </member>
  </compound>
  <compound kind="file">
    <name>README.md</name>
    <path>FIRE/</path>
    <filename>d9/dd6/_r_e_a_d_m_e_8md.html</filename>
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
      <type>List&lt; string &gt;</type>
      <name>KeyWords</name>
      <anchorfile>d3/daf/class_available_keyword_configuration.html</anchorfile>
      <anchor>a1d31bfc22c535d0312b909e22cc0b1fa</anchor>
      <arglist></arglist>
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
  </compound>
  <compound kind="class">
    <name>FIREDatabaseStatusEntity</name>
    <filename>d7/d1a/_f_i_r_e_database_8cs.html</filename>
    <anchor>da/d80/class_f_i_r_e_database_status_entity</anchor>
    <member kind="variable">
      <type>const long</type>
      <name>SingletonId</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>ad8c3e4b8fe9d2f2356f2e895acfb27d5</anchor>
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
    <name>FIREFileMetaDataEntity</name>
    <filename>d7/d1a/_f_i_r_e_database_8cs.html</filename>
    <anchor>d3/d8c/class_f_i_r_e_file_meta_data_entity</anchor>
    <member kind="variable">
      <type>long</type>
      <name>Id</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a24fa15ebbf058d55281483c98eff1011</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>set</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a125a78b6fd8538b28aed0d46a937df54</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>long</type>
      <name>RecordId</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a7d59b24dde38def90481d6fab45ef853</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>FIREDbRecordEntity</type>
      <name>Record</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>aa506e060fb561902dc9162eecd9a920c</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>Key</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a10eea546d40fa7cda5ea6d41e85a05fe</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>Value</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>af9721f21ae4764fddc766c89e69cb961</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>TypeName</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a4bd19e2114fcf0c67c48d30b7fbe4c18</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>DataSource</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a5a4b2e99a3849c340ce9bd923cd2866d</anchor>
      <arglist></arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>FIREStatusRecord</name>
    <filename>d7/d1a/_f_i_r_e_database_8cs.html</filename>
    <anchor>d7/d12/class_f_i_r_e_status_record</anchor>
    <member kind="variable">
      <type>long</type>
      <name>Id</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>ae93f2c0c22bf9258a83055a9d62f27b5</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type></type>
      <name>set</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a5e2d1bb0b9d6ca8fb1ee14045f90fea2</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>Status</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a1fb654166a27d34c5c9fdeba39ecb6e8</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>bool</type>
      <name>Valid</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>af2f9e15d9f5d60ca0c98f599ed8a6f38</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>DateTime</type>
      <name>TimeStamp</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a8b9be2c6dcf488eda86485059b17a1d1</anchor>
      <arglist></arglist>
    </member>
    <member kind="variable">
      <type>string</type>
      <name>ErrorMessage</name>
      <anchorfile>d7/d1a/_f_i_r_e_database_8cs.html</anchorfile>
      <anchor>a1c97b15df88db29fa409870c6c083e88</anchor>
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
  </compound>
  <compound kind="dir">
    <name>FIRE/FIRE</name>
    <path>FIRE/FIRE/</path>
    <filename>dir_027ae1c642818204f1d2b0e289285598.html</filename>
    <file>FIRECatalog.cs</file>
    <file>FIREConfigration.cs</file>
    <file>FIREDatabase.cs</file>
  </compound>
  <compound kind="dir">
    <name>FIRE/FIRE.Console</name>
    <path>FIRE/FIRE.Console/</path>
    <filename>dir_34712068e75e4f8a851a4fa1ab20db3f.html</filename>
    <file>Program.cs</file>
  </compound>
  <compound kind="page">
    <name>index</name>
    <title>FIRE — File Information Reorganizer and Extractor</title>
    <filename>index.html</filename>
    <docanchor file="index.html" title="Overview">intro_sec</docanchor>
    <docanchor file="index.html" title="Architecture">arch_sec</docanchor>
    <docanchor file="index.html" title="Three-Step Workflow">workflow_sec</docanchor>
    <docanchor file="index.html" title="Step 1 — collect">step1</docanchor>
    <docanchor file="index.html" title="Step 2 — generate">step2</docanchor>
    <docanchor file="index.html" title="Step 3 — execute">step3</docanchor>
    <docanchor file="index.html" title="Configuration">config_sec</docanchor>
    <docanchor file="index.html" title="Keyword Selection">keywords_sec</docanchor>
    <docanchor file="index.html" title="Metadata Sources">meta_sources_sec</docanchor>
    <docanchor file="index.html" title="Template Placeholders">placeholders_sec</docanchor>
    <docanchor file="index.html" title="Sidecar Files">sidecar_sec</docanchor>
    <docanchor file="index.html" title="License">license_sec</docanchor>
    <docanchor file="index.html" title="Links">links_sec</docanchor>
  </compound>
</tagfile>
