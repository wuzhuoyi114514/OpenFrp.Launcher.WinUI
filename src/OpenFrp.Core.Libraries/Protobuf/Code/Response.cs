// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: response.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace OpenFrp.Core.Libraries.Protobuf {

  /// <summary>Holder for reflection information generated from response.proto</summary>
  public static partial class ResponseReflection {

    #region Descriptor
    /// <summary>File descriptor for response.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static ResponseReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cg5yZXNwb25zZS5wcm90byKPAQoMUmVzcG9uc2VCYXNlEg8KB3N1Y2Nlc3MY",
            "ASABKAgSFAoHbWVzc2FnZRgCIAEoCUgAiAEBEhYKCWV4Y2VwdGlvbhgDIAEo",
            "CUgBiAEBEhAKCGxvZ3NKc29uGAQgAygJEhQKDHJ1bm5pbmdDb3VudBgFIAMo",
            "BUIKCghfbWVzc2FnZUIMCgpfZXhjZXB0aW9uQiKqAh9PcGVuRnJwLkNvcmUu",
            "TGlicmFyaWVzLlByb3RvYnVmYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::OpenFrp.Core.Libraries.Protobuf.ResponseBase), global::OpenFrp.Core.Libraries.Protobuf.ResponseBase.Parser, new[]{ "Success", "Message", "Exception", "LogsJson", "RunningCount" }, new[]{ "Message", "Exception" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class ResponseBase : pb::IMessage<ResponseBase>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<ResponseBase> _parser = new pb::MessageParser<ResponseBase>(() => new ResponseBase());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<ResponseBase> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::OpenFrp.Core.Libraries.Protobuf.ResponseReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public ResponseBase() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public ResponseBase(ResponseBase other) : this() {
      success_ = other.success_;
      message_ = other.message_;
      exception_ = other.exception_;
      logsJson_ = other.logsJson_.Clone();
      runningCount_ = other.runningCount_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public ResponseBase Clone() {
      return new ResponseBase(this);
    }

    /// <summary>Field number for the "success" field.</summary>
    public const int SuccessFieldNumber = 1;
    private bool success_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Success {
      get { return success_; }
      set {
        success_ = value;
      }
    }

    /// <summary>Field number for the "message" field.</summary>
    public const int MessageFieldNumber = 2;
    private string message_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string Message {
      get { return message_ ?? ""; }
      set {
        message_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "message" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasMessage {
      get { return message_ != null; }
    }
    /// <summary>Clears the value of the "message" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearMessage() {
      message_ = null;
    }

    /// <summary>Field number for the "exception" field.</summary>
    public const int ExceptionFieldNumber = 3;
    private string exception_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string Exception {
      get { return exception_ ?? ""; }
      set {
        exception_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "exception" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasException {
      get { return exception_ != null; }
    }
    /// <summary>Clears the value of the "exception" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearException() {
      exception_ = null;
    }

    /// <summary>Field number for the "logsJson" field.</summary>
    public const int LogsJsonFieldNumber = 4;
    private static readonly pb::FieldCodec<string> _repeated_logsJson_codec
        = pb::FieldCodec.ForString(34);
    private readonly pbc::RepeatedField<string> logsJson_ = new pbc::RepeatedField<string>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<string> LogsJson {
      get { return logsJson_; }
    }

    /// <summary>Field number for the "runningCount" field.</summary>
    public const int RunningCountFieldNumber = 5;
    private static readonly pb::FieldCodec<int> _repeated_runningCount_codec
        = pb::FieldCodec.ForInt32(42);
    private readonly pbc::RepeatedField<int> runningCount_ = new pbc::RepeatedField<int>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<int> RunningCount {
      get { return runningCount_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as ResponseBase);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(ResponseBase other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Success != other.Success) return false;
      if (Message != other.Message) return false;
      if (Exception != other.Exception) return false;
      if(!logsJson_.Equals(other.logsJson_)) return false;
      if(!runningCount_.Equals(other.runningCount_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (Success != false) hash ^= Success.GetHashCode();
      if (HasMessage) hash ^= Message.GetHashCode();
      if (HasException) hash ^= Exception.GetHashCode();
      hash ^= logsJson_.GetHashCode();
      hash ^= runningCount_.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (Success != false) {
        output.WriteRawTag(8);
        output.WriteBool(Success);
      }
      if (HasMessage) {
        output.WriteRawTag(18);
        output.WriteString(Message);
      }
      if (HasException) {
        output.WriteRawTag(26);
        output.WriteString(Exception);
      }
      logsJson_.WriteTo(output, _repeated_logsJson_codec);
      runningCount_.WriteTo(output, _repeated_runningCount_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (Success != false) {
        output.WriteRawTag(8);
        output.WriteBool(Success);
      }
      if (HasMessage) {
        output.WriteRawTag(18);
        output.WriteString(Message);
      }
      if (HasException) {
        output.WriteRawTag(26);
        output.WriteString(Exception);
      }
      logsJson_.WriteTo(ref output, _repeated_logsJson_codec);
      runningCount_.WriteTo(ref output, _repeated_runningCount_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (Success != false) {
        size += 1 + 1;
      }
      if (HasMessage) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Message);
      }
      if (HasException) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Exception);
      }
      size += logsJson_.CalculateSize(_repeated_logsJson_codec);
      size += runningCount_.CalculateSize(_repeated_runningCount_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(ResponseBase other) {
      if (other == null) {
        return;
      }
      if (other.Success != false) {
        Success = other.Success;
      }
      if (other.HasMessage) {
        Message = other.Message;
      }
      if (other.HasException) {
        Exception = other.Exception;
      }
      logsJson_.Add(other.logsJson_);
      runningCount_.Add(other.runningCount_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            Success = input.ReadBool();
            break;
          }
          case 18: {
            Message = input.ReadString();
            break;
          }
          case 26: {
            Exception = input.ReadString();
            break;
          }
          case 34: {
            logsJson_.AddEntriesFrom(input, _repeated_logsJson_codec);
            break;
          }
          case 42:
          case 40: {
            runningCount_.AddEntriesFrom(input, _repeated_runningCount_codec);
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            Success = input.ReadBool();
            break;
          }
          case 18: {
            Message = input.ReadString();
            break;
          }
          case 26: {
            Exception = input.ReadString();
            break;
          }
          case 34: {
            logsJson_.AddEntriesFrom(ref input, _repeated_logsJson_codec);
            break;
          }
          case 42:
          case 40: {
            runningCount_.AddEntriesFrom(ref input, _repeated_runningCount_codec);
            break;
          }
        }
      }
    }
    #endif

  }

  #endregion

}

#endregion Designer generated code
