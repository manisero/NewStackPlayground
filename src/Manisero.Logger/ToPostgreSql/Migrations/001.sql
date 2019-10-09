CREATE TABLE Logger.LogType
(
  Code uuid not null,
  Level smallint not null,
  MessageTemplate text not null,
  CONSTRAINT PK_LogType PRIMARY KEY (Code)
);

CREATE TABLE Logger.Log
(
  Timestamp timestamp not null,
  TypeCode uuid not null,
  Properties jsonb,
  Exception text
) PARTITION BY RANGE (Timestamp);

CREATE VIEW Logger.LogWithType AS
SELECT
  l.Timestamp,
  l.TypeCode,
  lt.Level,
  lt.MessageTemplate,
  l.Properties,
  l.Exception
FROM logger.Log l
LEFT JOIN logger.LogType lt ON l.TypeCode = lt.Code;

CREATE TABLE Logger.Log_201809 PARTITION OF Logger.Log FOR VALUES FROM ('2018-09-01') TO ('2018-10-01');
CREATE INDEX IX_Log_201809_Timestamp ON Logger.Log_201809 (Timestamp);

CREATE TABLE Logger.Log_201810 PARTITION OF Logger.Log FOR VALUES FROM ('2018-10-01') TO ('2018-11-01');
CREATE INDEX IX_Log_201810_Timestamp ON Logger.Log_201810 (Timestamp);

CREATE TABLE Logger.Log_201811 PARTITION OF Logger.Log FOR VALUES FROM ('2018-11-01') TO ('2018-12-01');
CREATE INDEX IX_Log_201811_Timestamp ON Logger.Log_201811 (Timestamp);

CREATE TABLE Logger.Log_201812 PARTITION OF Logger.Log FOR VALUES FROM ('2018-12-01') TO ('2019-01-01');
CREATE INDEX IX_Log_201812_Timestamp ON Logger.Log_201812 (Timestamp);

CREATE TABLE Logger.Log_201901 PARTITION OF Logger.Log FOR VALUES FROM ('2019-01-01') TO ('2019-02-01');
CREATE INDEX IX_Log_201901_Timestamp ON Logger.Log_201901 (Timestamp);

CREATE TABLE Logger.Log_201902 PARTITION OF Logger.Log FOR VALUES FROM ('2019-02-01') TO ('2019-03-01');
CREATE INDEX IX_Log_201902_Timestamp ON Logger.Log_201902 (Timestamp);

CREATE TABLE Logger.Log_201903 PARTITION OF Logger.Log FOR VALUES FROM ('2019-03-01') TO ('2019-04-01');
CREATE INDEX IX_Log_201903_Timestamp ON Logger.Log_201903 (Timestamp);

CREATE TABLE Logger.Log_201904 PARTITION OF Logger.Log FOR VALUES FROM ('2019-04-01') TO ('2019-05-01');
CREATE INDEX IX_Log_201904_Timestamp ON Logger.Log_201904 (Timestamp);

CREATE TABLE Logger.Log_201905 PARTITION OF Logger.Log FOR VALUES FROM ('2019-05-01') TO ('2019-06-01');
CREATE INDEX IX_Log_201905_Timestamp ON Logger.Log_201905 (Timestamp);

CREATE TABLE Logger.Log_201906 PARTITION OF Logger.Log FOR VALUES FROM ('2019-06-01') TO ('2019-07-01');
CREATE INDEX IX_Log_201906_Timestamp ON Logger.Log_201906 (Timestamp);

CREATE TABLE Logger.Log_201907 PARTITION OF Logger.Log FOR VALUES FROM ('2019-07-01') TO ('2019-08-01');
CREATE INDEX IX_Log_201907_Timestamp ON Logger.Log_201907 (Timestamp);

CREATE TABLE Logger.Log_201908 PARTITION OF Logger.Log FOR VALUES FROM ('2019-08-01') TO ('2019-09-01');
CREATE INDEX IX_Log_201908_Timestamp ON Logger.Log_201908 (Timestamp);

CREATE TABLE Logger.Log_201909 PARTITION OF Logger.Log FOR VALUES FROM ('2019-09-01') TO ('2019-10-01');
CREATE INDEX IX_Log_201909_Timestamp ON Logger.Log_201909 (Timestamp);
