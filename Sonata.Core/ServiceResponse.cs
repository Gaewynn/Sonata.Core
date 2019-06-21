#region Namespace Sonata.Core
//	TODO
#endregion

using Microsoft.Extensions.Logging;
using Sonata.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Sonata.Core
{
    /// <summary>
    /// A structure used to send back a common result structure to the client after a Web API call.
    /// </summary>
    [DataContract(Name = "serviceResponse")]
    public class ServiceResponse
    {
        #region Members

        private bool _isInfos;
        private bool _isDebugs;
        private bool _isWarnings;
        private bool _isErrors;
        private bool _isExceptions;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating the result returned to the client.
        /// </summary>
        [DataMember(Name = "result")]
        public object Result { get; set; }

        /// <summary>
        /// Gets a list of <see cref="LogLevel.Information"/> <see cref="Log"/> generated during the server process.
        /// </summary>
        [DataMember(Name = "infos")]
        public List<Log> Infos { get; set; }

        /// <summary>
        /// Gets a list of <see cref="LogLevel.Debug"/> and <see cref="LogLevel.Trace"/> <see cref="Log"/> generated during the server process.
        /// </summary>
        [DataMember(Name = "debugs")]
        public List<Log> Debugs { get; set; }

        /// <summary>
        /// Gets a list of <see cref="LogLevel.Warning"/> <see cref="Log"/> generated during the server process.
        /// </summary>
        [DataMember(Name = "warnings")]
        public List<Log> Warnings { get; set; }

        /// <summary>
        /// Gets a list of <see cref="LogLevel.Error"/> <see cref="Log"/> generated during the server process.
        /// </summary>
        [DataMember(Name = "errors")]
        public List<Log> Errors { get; set; }

        /// <summary>
        /// Gets a list of <see cref="LogLevel.Critical"/> <see cref="Log"/> that occured during the server process.
        /// </summary>
        [DataMember(Name = "exceptions")]
        public List<Log> Exceptions { get; set; }

        /// <summary>
        /// Gets a value indicating whether the current <see cref="ServiceResponse"/> contains <see cref="LogLevel.Information"/> <see cref="Log"/>.
        /// </summary>
        [DataMember(Name = "isInfos")]
        public bool IsInfos
        {
            get => _isInfos || Infos.Any();
            set => _isInfos = value;
        }

        /// <summary>
        /// Gets a value indicating whether the current <see cref="ServiceResponse"/> contains <see cref="LogLevel.Debug"/> <see cref="Log"/>.
        /// </summary>
        [DataMember(Name = "isDebugs")]
        public bool IsDebugs
        {
            get => _isDebugs || Debugs.Any();
            set => _isDebugs = value;
        }

        /// <summary>
        /// Gets a value indicating whether the current <see cref="ServiceResponse"/> contains <see cref="LogLevel.Warning"/> <see cref="Log"/>.
        /// </summary>
        [DataMember(Name = "isWarnings")]
        public bool IsWarnings
        {
            get => _isWarnings || Warnings.Any();
            set => _isWarnings = value;
        }

        /// <summary>
        /// Gets a value indicating whether the current <see cref="ServiceResponse"/> contains <see cref="LogLevel.Error"/> <see cref="Log"/>.
        /// </summary>
        [DataMember(Name = "isErrors")]
        public bool IsErrors
        {
            get => _isErrors || Errors.Any();
            set => _isErrors = value;
        }

        /// <summary>
        /// Gets a value indicating whether the current <see cref="ServiceResponse"/> contains <see cref="LogLevel.Critical"/> <see cref="Log"/>.
        /// </summary>
        [DataMember(Name = "isExceptions")]
        public bool IsExceptions
        {
            get => _isExceptions || Exceptions.Any();
            set => _isExceptions = value;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponse"/> class.
        /// </summary>
        public ServiceResponse()
        {
            Infos = new List<Log>();
            Debugs = new List<Log>();
            Warnings = new List<Log>();
            Errors = new List<Log>();
            Exceptions = new List<Log>();
        }

        #endregion

        #region Methods

        public void SetLogs(Dictionary<LogLevel, List<string>> logs)
        {
            if (logs == null)
                throw new ArgumentNullException(nameof(logs));

            SetLogs(logs.ToDictionary(
                e => e.Key,
                e => e.Value.Select(logMessage => Log.Build(e.Key, logMessage))));
        }

        /// <summary>
        /// Adds the specified <paramref name="logs"/> to the current <see cref="ServiceResponse"/>.
        /// </summary>
        /// <param name="logs">A list of <see cref="Log"/> to add to the current <see cref="ServiceResponse"/>.</param>
        /// <remarks>Existing logs are not overridden.</remarks>
        public void SetLogs(Dictionary<LogLevel, IEnumerable<Log>> logs)
        {
            if (logs == null)
                throw new ArgumentNullException(nameof(logs));

            if (logs.ContainsKey(LogLevel.Information) && logs[LogLevel.Information] != null && logs[LogLevel.Information].Any())
                Infos.AddRange(logs[LogLevel.Information]);

            if (logs.ContainsKey(LogLevel.Debug) && logs[LogLevel.Debug] != null && logs[LogLevel.Debug].Any())
                Debugs.AddRange(logs[LogLevel.Debug]);

            if (logs.ContainsKey(LogLevel.Warning) && logs[LogLevel.Warning] != null && logs[LogLevel.Warning].Any())
                Warnings.AddRange(logs[LogLevel.Warning]);

            if (logs.ContainsKey(LogLevel.Error) && logs[LogLevel.Error] != null && logs[LogLevel.Error].Any())
                Errors.AddRange(logs[LogLevel.Error]);

            if (logs.ContainsKey(LogLevel.Critical) && logs[LogLevel.Critical] != null && logs[LogLevel.Critical].Any())
                Exceptions.AddRange(logs[LogLevel.Critical]);
        }

        public void Add(LogLevel logLevel, string message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            Add(Log.Build(logLevel, message));
        }

        /// <summary>
        /// Adds the specified <paramref name="log"/> to the current <see cref="ServiceResponse"/>.
        /// </summary>
        /// <param name="log">The <see cref="Log"/> to add to the current <see cref="ServiceResponse"/>.</param>
        public void Add(Log log)
        {
            if (log == null)
                throw new ArgumentNullException(nameof(log));

            if (log.Level == LogLevel.Trace || log.Level == LogLevel.Debug)
                Debugs.Add(log);

            if (log.Level == LogLevel.Information)
                Infos.Add(log);

            if (log.Level == LogLevel.Warning)
                Warnings.Add(log);

            if (log.Level == LogLevel.Error)
                Errors.Add(log);

            if (log.Level == LogLevel.Critical)
                Exceptions.Add(log);
        }

        public void AddTrace(string message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            Add(LogLevel.Trace, new string[1] { message });
        }

        public void AddDebug(string message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            Add(LogLevel.Debug, new string[1] { message });
        }

        public void AddInformation(string message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            Add(LogLevel.Information, new string[1] { message });
        }

        public void AddWarning(string message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            Add(LogLevel.Warning, new string[1] { message });
        }

        public void AddError(string message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            Add(LogLevel.Error, new string[1] { message });
        }

        public void AddCritical(string message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            Add(LogLevel.Critical, new string[1] { message });
        }

        public void AddTrace(IEnumerable<string> messages)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            Add(LogLevel.Trace, messages);
        }

        public void AddDebug(IEnumerable<string> messages)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            Add(LogLevel.Debug, messages);
        }

        public void AddInformation(IEnumerable<string> messages)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            Add(LogLevel.Information, messages);
        }

        public void AddWarning(IEnumerable<string> messages)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            Add(LogLevel.Warning, messages);
        }

        public void AddError(IEnumerable<string> messages)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            Add(LogLevel.Error, messages);
        }

        public void AddCritical(IEnumerable<string> messages)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            Add(LogLevel.Critical, messages);
        }

        public void Add(LogLevel logLevel, IEnumerable<string> messages)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            Add(messages.Select(e => Log.Build(logLevel, e)));
        }

        /// <summary>
        /// Adds the specified Logs to the current <see cref="ServiceResponse"/>.
        /// </summary>
        /// <param name="logs">The Logs to add to the current <see cref="ServiceResponse"/>.</param>
        public void Add(IEnumerable<Log> logs)
        {
            if (logs == null)
                throw new ArgumentNullException(nameof(logs));

            foreach (var log in logs)
                Add(log);
        }

        public List<string> GetLogsMessages(params LogLevel[] filters)
        {
            var messages = new List<string>();
            if (filters == null)
                return messages;

            if (filters.Contains(LogLevel.Critical))
                messages.AddRange(Exceptions != null ? Exceptions.Select(e => e.Message) : new List<string>());

            if (filters.Contains(LogLevel.Error))
                messages.AddRange(Errors != null ? Errors.Select(e => e.Message) : new List<string>());

            if (filters.Contains(LogLevel.Warning))
                messages.AddRange(Warnings != null ? Warnings.Select(e => e.Message) : new List<string>());

            if (filters.Contains(LogLevel.Information))
                messages.AddRange(Infos != null ? Infos.Select(e => e.Message) : new List<string>());

            if (filters.Contains(LogLevel.Debug))
                messages.AddRange(Debugs != null ? Debugs.Select(e => e.Message) : new List<string>());

            return messages;
        }

        public bool IsAnyLogs()
        {
            return GetLogsMessages(LogLevel.Critical, LogLevel.Error, LogLevel.Warning, LogLevel.Information, LogLevel.Debug).Any();
        }

        public bool IsAnyLogs(params LogLevel[] filters)
        {
            return GetLogsMessages(filters).Any();
        }

        #endregion
    }

    [DataContract(Name = "serviceResponse")]
    public class ServiceResponse<T>
    {
        #region Members

        private bool _isInfos;
        private bool _isDebugs;
        private bool _isWarnings;
        private bool _isErrors;
        private bool _isExceptions;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating the result returned to the client.
        /// </summary>
        [DataMember(Name = "result")]
        public T Result { get; set; }

        /// <summary>
        /// Gets a list of <see cref="LogLevel.Information"/> <see cref="Log"/> generated during the server process.
        /// </summary>
        [DataMember(Name = "infos")]
        public List<Log> Infos { get; set; }

        /// <summary>
        /// Gets a list of <see cref="LogLevel.Debug"/> and <see cref="LogLevel.Trace"/> <see cref="Log"/> generated during the server process.
        /// </summary>
        [DataMember(Name = "debugs")]
        public List<Log> Debugs { get; set; }

        /// <summary>
        /// Gets a list of <see cref="LogLevel.Warning"/> <see cref="Log"/> generated during the server process.
        /// </summary>
        [DataMember(Name = "warnings")]
        public List<Log> Warnings { get; set; }

        /// <summary>
        /// Gets a list of <see cref="LogLevel.Error"/> <see cref="Log"/> generated during the server process.
        /// </summary>
        [DataMember(Name = "errors")]
        public List<Log> Errors { get; set; }

        /// <summary>
        /// Gets a list of <see cref="LogLevel.Critical"/> <see cref="Log"/> that occured during the server process.
        /// </summary>
        [DataMember(Name = "exceptions")]
        public List<Log> Exceptions { get; set; }

        /// <summary>
        /// Gets a value indicating whether the current <see cref="ServiceResponse"/> contains <see cref="LogLevel.Information"/> <see cref="Log"/>.
        /// </summary>
        [DataMember(Name = "isInfos")]
        public bool IsInfos
        {
            get => _isInfos || Infos.Any();
            set => _isInfos = value;
        }

        /// <summary>
        /// Gets a value indicating whether the current <see cref="ServiceResponse"/> contains <see cref="LogLevel.Debug"/> <see cref="Log"/>.
        /// </summary>
        [DataMember(Name = "isDebugs")]
        public bool IsDebugs
        {
            get => _isDebugs || Debugs.Any();
            set => _isDebugs = value;
        }

        /// <summary>
        /// Gets a value indicating whether the current <see cref="ServiceResponse"/> contains <see cref="LogLevel.Warning"/> <see cref="Log"/>.
        /// </summary>
        [DataMember(Name = "isWarnings")]
        public bool IsWarnings
        {
            get => _isWarnings || Warnings.Any();
            set => _isWarnings = value;
        }

        /// <summary>
        /// Gets a value indicating whether the current <see cref="ServiceResponse"/> contains <see cref="LogLevel.Error"/> <see cref="Log"/>.
        /// </summary>
        [DataMember(Name = "isErrors")]
        public bool IsErrors
        {
            get => _isErrors || Errors.Any();
            set => _isErrors = value;
        }

        /// <summary>
        /// Gets a value indicating whether the current <see cref="ServiceResponse"/> contains <see cref="LogLevel.Critical"/> <see cref="Log"/>.
        /// </summary>
        [DataMember(Name = "isExceptions")]
        public bool IsExceptions
        {
            get => _isExceptions || Exceptions.Any();
            set => _isExceptions = value;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponse"/> class.
        /// </summary>
        public ServiceResponse()
        {
            Infos = new List<Log>();
            Debugs = new List<Log>();
            Warnings = new List<Log>();
            Errors = new List<Log>();
            Exceptions = new List<Log>();
        }

        #endregion

        #region Methods

        public void SetLogs(Dictionary<LogLevel, List<string>> logs)
        {
            if (logs == null)
                throw new ArgumentNullException(nameof(logs));

            SetLogs(logs.ToDictionary(
                e => e.Key,
                e => e.Value.Select(logMessage => Log.Build(e.Key, logMessage))));
        }

        /// <summary>
        /// Adds the specified <paramref name="logs"/> to the current <see cref="ServiceResponse"/>.
        /// </summary>
        /// <param name="logs">A list of <see cref="Log"/> to add to the current <see cref="ServiceResponse"/>.</param>
        /// <remarks>Existing logs are not overridden.</remarks>
        public void SetLogs(Dictionary<LogLevel, IEnumerable<Log>> logs)
        {
            if (logs == null)
                throw new ArgumentNullException(nameof(logs));

            if (logs.ContainsKey(LogLevel.Information) && logs[LogLevel.Information] != null && logs[LogLevel.Information].Any())
                Infos.AddRange(logs[LogLevel.Information]);

            if (logs.ContainsKey(LogLevel.Debug) && logs[LogLevel.Debug] != null && logs[LogLevel.Debug].Any())
                Debugs.AddRange(logs[LogLevel.Debug]);

            if (logs.ContainsKey(LogLevel.Warning) && logs[LogLevel.Warning] != null && logs[LogLevel.Warning].Any())
                Warnings.AddRange(logs[LogLevel.Warning]);

            if (logs.ContainsKey(LogLevel.Error) && logs[LogLevel.Error] != null && logs[LogLevel.Error].Any())
                Errors.AddRange(logs[LogLevel.Error]);

            if (logs.ContainsKey(LogLevel.Critical) && logs[LogLevel.Critical] != null && logs[LogLevel.Critical].Any())
                Exceptions.AddRange(logs[LogLevel.Critical]);
        }

        public void Add(LogLevel logLevel, string message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            Add(Log.Build(logLevel, message));
        }

        /// <summary>
        /// Adds the specified <paramref name="log"/> to the current <see cref="ServiceResponse"/>.
        /// </summary>
        /// <param name="log">The <see cref="Log"/> to add to the current <see cref="ServiceResponse"/>.</param>
        public void Add(Log log)
        {
            if (log == null)
                throw new ArgumentNullException(nameof(log));

            if (log.Level == LogLevel.Trace || log.Level == LogLevel.Debug)
                Debugs.Add(log);

            if (log.Level == LogLevel.Information)
                Infos.Add(log);

            if (log.Level == LogLevel.Warning)
                Warnings.Add(log);

            if (log.Level == LogLevel.Error)
                Errors.Add(log);

            if (log.Level == LogLevel.Critical)
                Exceptions.Add(log);
        }

        public void AddTrace(string message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            Add(LogLevel.Trace, new string[1] { message });
        }

        public void AddDebug(string message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            Add(LogLevel.Debug, new string[1] { message });
        }

        public void AddInformation(string message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            Add(LogLevel.Information, new string[1] { message });
        }

        public void AddWarning(string message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            Add(LogLevel.Warning, new string[1] { message });
        }

        public void AddError(string message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            Add(LogLevel.Error, new string[1] { message });
        }

        public void AddCritical(string message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            Add(LogLevel.Critical, new string[1] { message });
        }

        public void AddTrace(IEnumerable<string> messages)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            Add(LogLevel.Trace, messages);
        }

        public void AddDebug(IEnumerable<string> messages)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            Add(LogLevel.Debug, messages);
        }

        public void AddInformation(IEnumerable<string> messages)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            Add(LogLevel.Information, messages);
        }

        public void AddWarning(IEnumerable<string> messages)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            Add(LogLevel.Warning, messages);
        }

        public void AddError(IEnumerable<string> messages)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            Add(LogLevel.Error, messages);
        }

        public void AddCritical(IEnumerable<string> messages)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            Add(LogLevel.Critical, messages);
        }

        public void Add(LogLevel logLevel, IEnumerable<string> messages)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            Add(messages.Select(e => Log.Build(logLevel, e)));
        }

        /// <summary>
        /// Adds the specified Logs to the current <see cref="ServiceResponse"/>.
        /// </summary>
        /// <param name="logs">The Logs to add to the current <see cref="ServiceResponse"/>.</param>
        public void Add(IEnumerable<Log> logs)
        {
            if (logs == null)
                throw new ArgumentNullException(nameof(logs));

            foreach (var log in logs)
                Add(log);
        }

        public List<string> GetLogsMessages(params LogLevel[] filters)
        {
            var messages = new List<string>();
            if (filters == null)
                return messages;

            if (filters.Contains(LogLevel.Critical))
                messages.AddRange(Exceptions != null ? Exceptions.Select(e => e.Message) : new List<string>());

            if (filters.Contains(LogLevel.Error))
                messages.AddRange(Errors != null ? Errors.Select(e => e.Message) : new List<string>());

            if (filters.Contains(LogLevel.Warning))
                messages.AddRange(Warnings != null ? Warnings.Select(e => e.Message) : new List<string>());

            if (filters.Contains(LogLevel.Information))
                messages.AddRange(Infos != null ? Infos.Select(e => e.Message) : new List<string>());

            if (filters.Contains(LogLevel.Debug))
                messages.AddRange(Debugs != null ? Debugs.Select(e => e.Message) : new List<string>());

            return messages;
        }

        public bool IsAnyLogs()
        {
            return GetLogsMessages(LogLevel.Critical, LogLevel.Error, LogLevel.Warning, LogLevel.Information, LogLevel.Debug).Any();
        }

        public bool IsAnyLogs(params LogLevel[] filters)
        {
            return GetLogsMessages(filters).Any();
        }

        #endregion
    }
}