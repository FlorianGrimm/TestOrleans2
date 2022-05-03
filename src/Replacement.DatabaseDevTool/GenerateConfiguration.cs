using Brimborium.GenerateStoredProcedure;

namespace Replacement.DatabaseDevTool {
    public class GenerateConfiguration : Configuration {
        // public readonly RenderTemplate<TableInfo> SelectColumnsParameterTempate;

        public readonly RenderTemplate<TableInfo> SelectColumnsParameterPKTempate;

        public readonly RenderTemplate<TableInfo> ConditionColumnsParameterPKTempate;

        public readonly RenderTemplate<TableInfo> AtTableResultPKTempate;

        public readonly RenderTemplate<TableInfo> AtTableResultTempate;

        public readonly RenderTemplate<TableInfo> SelectAtTableResultTemplate;

        public readonly RenderTemplate<TableInfo> InsertIntoTableOutputAtTableResultValuesParameterTemplate;

        public readonly RenderTemplate<TableInfo> InsertIntoTableValuesParameterTemplate;

        public readonly RenderTemplate<TableInfo> SelectPKTempateBody;

        public readonly RenderTemplate<TableInfo> SelectPKTempate;

        public readonly RenderTemplate<TableInfo> SelectAtTimeTempate;

        public readonly RenderTemplate<ForeignKeyInfo> SelectByReferencedPKTempate;

        public record TableDataHistory(
            TableInfo TableData,
            TableInfo TableHistory,
            List<(ColumnInfo columnData, ColumnInfo columnHistory)> ColumnPairs);

        public record TableDataFK(
            TableInfo TableData,
            List<ForeignKeyInfo> ForeignKeyReferenced);

        public readonly RenderTemplate<TableDataHistory> UpdateTempate;

        public readonly RenderTemplate<TableDataHistory> DeletePKTempate;

        public readonly RenderTemplate<TableDataHistory> DeletePKTempateParameter;



        public GenerateConfiguration() {
            //this.SelectColumnsParameterTempate = new RenderTemplate<TableInfo>(
            //    NameFn: (t) => $"SelectColumnsParameterTempate.{t.GetNameQ()}",
            //    Render: (data, ctxt) => {
            //        this.KT.Columns.Render(data.Columns, ctxt);
            //        this.KT.ColumnRowversion.Render(data, ctxt);
            //    });

            this.SelectColumnsParameterPKTempate = new RenderTemplate<TableInfo>(
                NameFn: (t) => $"{nameof(SelectColumnsParameterPKTempate)}.{t.GetNameQ()}",
                Render: (data, ctxt) => {
                    ctxt.AppendList(
                        data.IndexPrimaryKey.Columns,
                        (data, ctxt) => {
                            ctxt.AppendPartsLine(
                                data.GetNameQ(),
                                ctxt.IfNotLast(","));
                        });
                });

            this.ConditionColumnsParameterPKTempate = new RenderTemplate<TableInfo>(
                NameFn: (t) => $"{nameof(ConditionColumnsParameterPKTempate)}.{t.GetNameQ()}",
                Render: (data, ctxt) => {
                    ctxt.AppendList(
                        data.PrimaryKeyColumns,
                        (column, ctxt) => {
                            ctxt.AppendPartsLine(
                                ctxt.IfNotFirst(" AND "),
                                "(",
                                column.GetNamePrefixed("@"), " = ", column.GetNameQ(),
                                ")"
                                );
                        });
                });

            this.AtTableResultPKTempate = new RenderTemplate<TableInfo>(
                NameFn: (t) => $"{nameof(AtTableResultPKTempate)}.{t.GetNameQ()}",
                Render: (data, ctxt) => {
                    ctxt.AppendLine($"DECLARE @ResultPK_{data.Schema}_{data.Name} AS TABLE (");
                    ctxt.AppendIndented(
                        data,
                        (data, ctxt) => {
                            ctxt.AppendList(
                                data.IndexPrimaryKey.Columns,
                                (data, ctxt) => {
                                    ctxt.AppendPartsLine(
                                        data.GetNameQ(),
                                        " ",
                                        data.GetSqlDataType(true),
                                        ctxt.IfNotLast(",")
                                        );
                                });

                            ctxt.AppendLine("PRIMARY KEY CLUSTERED (");
                            ctxt.AppendIndented(
                                data,
                                (data, ctxt) => {
                                    ctxt.AppendList(
                                    data.IndexPrimaryKey.Columns,
                                    (data, ctxt) => {
                                        ctxt.AppendPartsLine(
                                            data.GetNameQ(),
                                            ctxt.IfNotLast(",")
                                            );
                                    });
                                });
                            ctxt.AppendLine("));");
                        });
                });

            this.AtTableResultTempate = new RenderTemplate<TableInfo>(
                NameFn: (t) => $"{nameof(AtTableResultTempate)}.{t.GetNameQ()}",
                Render: (data, ctxt) => {
                    ctxt.AppendLine($"DECLARE @Result_{data.Schema}_{data.Name} AS TABLE (");
                    ctxt.AppendIndented(
                        data,
                        (data, ctxt) => {
                            ctxt.AppendList(
                                data.ColumnsWithRowversion,
                                (data, ctxt) => {
                                    ctxt.AppendPartsLine(
                                        data.GetNameQ(),
                                        " ",
                                        data.GetParameterSqlDataType(true),
                                        ctxt.IfNotLast(",")
                                        );
                                });

                            ctxt.AppendLine("PRIMARY KEY CLUSTERED (");
                            ctxt.AppendIndented(
                                data,
                                (data, ctxt) => {
                                    ctxt.AppendList(
                                    data.IndexPrimaryKey.Columns,
                                    (data, ctxt) => {
                                        ctxt.AppendPartsLine(
                                            data.GetNameQ(),
                                            ctxt.IfNotLast(",")
                                            );
                                    });
                                });
                            ctxt.AppendLine("));");
                        });
                });

            this.SelectAtTableResultTemplate = new RenderTemplate<TableInfo>(
                NameFn: (t) => $"{nameof(SelectAtTableResultTemplate)}.{t.GetNameQ()}",
                Render: (data, ctxt) => {
                    KnownTemplates.SqlSelect(
                        data,
                        ctxt,
                        top: null,
                        columnsBlock: (data, ctxt) => {
                            this.KnownTemplates.SelectTableColumns.Render(data, ctxt);
                        },
                        fromBlock: (data, ctxt) => {
                            ctxt.AppendLine($"@Result_{data.Schema}_{data.Name}");
                        }
                        );
                });
            this.InsertIntoTableValuesParameterTemplate = new RenderTemplate<TableInfo>(
                NameFn: (t) => $"{nameof(InsertIntoTableValuesParameterTemplate)}.{t.GetNameQ()}",
                Render: (data, ctxt) => {

                    KnownTemplates.SqlInsertValues(
                        data,
                        ctxt,
                        target: data.GetNameQ(),
                        nameBlock: (data, ctxt) => {
                            ctxt.AppendList(
                                data.Columns,
                                (column, ctxt) => {
                                    ctxt.AppendPartsLine(column.GetNameQ(), ctxt.IfNotLast(","));
                                }
                                );
                        },
                        valuesBlock: (data, ctxt) => {
                            ctxt.AppendList(
                                data.Columns,
                                (column, ctxt) => {
                                    ctxt.AppendPartsLine(column.GetNamePrefixed("@"), ctxt.IfNotLast(","));
                                });
                        }
                        );
                });
            this.InsertIntoTableOutputAtTableResultValuesParameterTemplate = new RenderTemplate<TableInfo>(
                NameFn: (t) => $"{nameof(InsertIntoTableOutputAtTableResultValuesParameterTemplate)}.{t.GetNameQ()}",
                Render: (data, ctxt) => {

                    KnownTemplates.SqlInsertValues(
                        data,
                        ctxt,
                        target: data.GetNameQ(),
                        nameBlock: (data, ctxt) => {
                            ctxt.AppendList(
                                data.Columns,
                                (column, ctxt) => {
                                    ctxt.AppendPartsLine(column.GetNameQ(), ctxt.IfNotLast(","));
                                }
                                );
                        },
                        valuesBlock: (data, ctxt) => {
                            ctxt.AppendList(
                                data.Columns,
                                (column, ctxt) => {
                                    ctxt.AppendPartsLine(column.GetNamePrefixed("@"), ctxt.IfNotLast(","));
                                });
                        },
                        outputBlock: (data, ctxt) => {
                            ctxt.AppendList(
                                data.ColumnsWithRowversion,
                                (column, ctxt) => {
                                    ctxt.AppendPartsLine(column.GetReadAsQ("INSERTED"), ctxt.IfNotLast(","));
                                });
                        },
                        outputIntoBlock: (data, ctxt) => {
                            ctxt.AppendLine($"@Result_{data.Schema}_{data.Name}");
                        }
                        );
                });

            this.SelectPKTempateBody = new RenderTemplate<TableInfo>(
                NameFn: (t) => $"SelectPKTempateBody.{t.GetNameQ()}",
                Render: (data, ctxt) => {
                    KnownTemplates.SqlSelect(
                             data,
                             ctxt,
                             top: 1,
                             columnsBlock: (data, ctxt) => {
                                 this.KnownTemplates.SelectTableColumns.Render(data, ctxt);
                             },
                             fromBlock: (data, ctxt) => {
                                 ctxt.AppendLine(data.GetNameQ());
                             },
                             whereBlock: (data, ctxt) => {
                                 ctxt.AppendList(
                                     data.PrimaryKeyColumns,
                                     (column, ctxt) => {
                                         ctxt.AppendPartsLine(
                                             ctxt.IfNotFirst(" AND "),
                                             "(",
                                             column.GetNamePrefixed("@"), " = ", column.GetNameQ(),
                                             ")"
                                             );
                                     });
                             });
                });

            this.SelectPKTempate = new RenderTemplate<TableInfo>(
                FileNameFn: RenderTemplateExtentsions.GetFileNameBind<TableInfo>(@"[Schema]\StoredProcedures\[Schema].[Name]SelectPK.sql"),
                Render: (data, ctxt) => {
                    KnownTemplates.SqlCreateProcedure(
                        data,
                        ctxt,
                        schema: data.Table.Schema,
                        name: $"{data.Table.Name}SelectPK",
                        parameter: (data, ctxt) => {
                            ctxt.RenderTemplate(
                                data.PrimaryKeyColumns
                                , this.KnownTemplates.ColumnsAsParameter);
                        },
                        bodyBlock: (data, ctxt) => {
                            this.SelectPKTempateBody.Render(data, ctxt);
                        });
                });

            this.SelectAtTimeTempate = new RenderTemplate<TableInfo>(
               NameFn: (t) => $"SelectAtTimeTempate.{t.GetNameQ()}",
               FileNameFn: RenderTemplateExtentsions.GetFileNameBind<TableInfo>(@"[Schema]\StoredProcedures\[Schema].[Name]SelectAtTime.sql"),
               Render: (data, ctxt) => {
                   //List<ColumnInfo> parameters = new List<ColumnInfo>();
                   //if (new string[] { "ExternalSourceId" }.SequenceEqual(data.PrimaryKeyColumns.Select(c => c.Name))) {
                   //    // skip
                   //} else {
                   //    parameters.AddRange(data.Columns.Where(c => c.Name == "ExternalSourceId"));
                   //}
                   KnownTemplates.SqlCreateProcedure(
                       data,
                       ctxt,
                       schema: data.Table.Schema,
                       name: $"{data.Table.Name}SelectAtTime",
                       parameter: (data, ctxt) => {
                           ctxt.AppendLine("@PointInTime DATETIME");

                           //ctxt.RenderTemplate(
                           //    parameters
                           //    , kt.ColumnsAsParameter
                           //    );
                       },
                       bodyBlock: (data, ctxt) => {
                           KnownTemplates.SqlSelect(
                               data,
                               ctxt,
                               top: null,
                               columnsBlock: (data, ctxt) => {
                                   ctxt.AppendList(
                                       data.Columns,
                                       (column, ctxt) => {
                                           ctxt.AppendPartsLine(
                                               column.GetNameQ(), ","
                                               );
                                       });
                                   ctxt.AppendLine(
                                       $"{data.ColumnRowversion.GetNameQ()} = CAST({data.ColumnRowversion.GetNameQ()} as BIGINT)");
                               },
                               fromBlock: (data, ctxt) => {
                                   ctxt.AppendLine(data.GetNameQ());
                               },
                               whereBlock: (data, ctxt) => {
                                   ctxt.Append("([ValidFrom] <= @PointInTime) AND (@PointInTime < [ValidTo])");

                               }
                               //whereBlock: parameters.Any() ? ((data, ctxt) => {
                               //    ctxt.AppendList(
                               //        data.PrimaryKeyColumns,
                               //        (column, ctxt) => {
                               //            ctxt.AppendPartsLine(
                               //                ctxt.IfNotFirst(" AND "),
                               //                "(",
                               //                column.GetNamePrefixed("@"), " = ", column.GetNameQ(),
                               //                ")"
                               //                );
                               //        });
                               //}) : null
                               );
                       });
               });


            this.SelectByReferencedPKTempate = new RenderTemplate<ForeignKeyInfo>(
                FileNameFn: (foreignKey, boundVariables)
                => RenderTemplateExtentsions.GetAbsoluteFilename(
                    $@"{foreignKey.TableInfo.Schema}\StoredProcedures\{foreignKey.TableInfo.Schema}.{foreignKey.TableInfo.Name}SelectByFK{foreignKey.GetShortName()}.sql",
                    boundVariables)
                ,
                Render: (foreignKey, ctxt) => {
                    KnownTemplates.SqlCreateProcedure(
                        foreignKey,
                        ctxt,
                        schema: foreignKey.TableInfo.Schema,
                        name: $"{foreignKey.TableInfo.Name}SelectByFK{foreignKey.GetShortName()}",
                        parameter: (data, ctxt) => {
                            ctxt.RenderTemplate(
                                data.ForeignKeyColumnsReferenced
                                , this.KnownTemplates.ColumnsAsParameter);
                        },
                        bodyBlock: (data, ctxt) => {
                            KnownTemplates.SqlSelect(
                                data,
                                ctxt,
                                top: null,
                                columnsBlock: (data, ctxt) => {
                                    this.KnownTemplates.Columns.Render(data.TableInfo.Columns, ctxt);
                                    this.KnownTemplates.ColumnRowversion.Render(data.TableInfo, ctxt);
                                },
                                fromBlock: (data, ctxt) => {
                                    ctxt.AppendLine(data.TableInfo.GetNameQ());
                                },
                                whereBlock: (data, ctxt) => {
                                    ctxt.AppendList(
                                        data.PairedColumns,
                                        (pairedColumn, ctxt) => {
                                            ctxt.AppendPartsLine(
                                                ctxt.IfNotFirst(" AND "),
                                                "(",
                                                pairedColumn.FKC.GetNamePrefixed("@"), " = ", pairedColumn.RefC.GetNameQ(),
                                                ")"
                                                );
                                        });
                                });
                        });
                });

            this.UpdateTempate = new RenderTemplate<TableDataHistory>(
                FileNameFn: RenderTemplateExtentsions.GetFileNameBind<TableDataHistory>(@"[Schema]\StoredProcedures\[Schema].[Name]Upsert.sql"),
                Render: (data, ctxt) => {
                    KnownTemplates.SqlCreateProcedure(
                        data,
                        ctxt,
                        schema: data.TableData.Table.Schema,
                        name: $"{data.TableData.Table.Name}Upsert",
                        parameter: (data, ctxt) => {
                            ctxt.RenderTemplate(
                                (columns: data.TableData.Columns, columnRowVersion: data.TableData.ColumnRowversion),
                                this.KnownTemplates.ColumnsAsParameterWithRowVersion
                                );
                        },
                        bodyBlock: (data, ctxt) => {
                            ctxt.RenderTemplate(
                                (columns: data.TableData.Columns, columnRowVersion: data.TableData.ColumnRowversion, prefix: "@Current"),
                                this.KnownTemplates.ColumnsAsDeclareParameterWithRowVersion
                                );
                            ctxt.AppendLine("DECLARE @ResultValue INT;");
                            ctxt.AppendLine("");
#if false
                            KnownTemplates.SqlIf(
                                data,
                                ctxt,
                                condition: (data, ctxt) => {
                                    ctxt.Append(data.TableData.ColumnRowversion.GetNamePrefixed("@Current")).Append(" > 0");
                                },
                                thenBlock: (data, ctxt) => {
                                    KnownTemplates.SqlSelect(
                                        data.TableData,
                                        ctxt,
                                        top: 1,
                                        columnsBlock: (data, ctxt) => {
                                            ctxt.AppendList(
                                                data.Columns,
                                                (column, ctxt) => {
                                                    ctxt.AppendPartsLine(
                                                        column.GetNamePrefixed("@Current"), " = ", column.GetNameQ(), ","
                                                        );
                                                });
                                            ctxt.AppendPartsLine(
                                                data.ColumnRowversion.GetNamePrefixed("@Current"),
                                                " = CAST(",
                                                data.ColumnRowversion.GetNameQ(),
                                                " as BIGINT)");
                                        },
                                        fromBlock: (data, ctxt) => {
                                            ctxt.AppendLine(data.GetNameQ());
                                        },
                                        whereBlock: (data, ctxt) => {
                                            ctxt.AppendList(
                                                data.PrimaryKeyColumns, // TODO FastPrimaryKeyColumns,
                                                (column, ctxt) => {
                                                    ctxt.AppendPartsLine(
                                                        ctxt.IfNotFirst(" AND "),
                                                        "(",
                                                        column.GetNamePrefixed("@"), " = ", column.GetNameQ(),
                                                        ")"
                                                        );
                                                });
                                        });
                                },
                                elseBlock: (data, ctxt) => {
                                    KnownTemplates.SqlSelect(
                                        data.TableData,
                                        ctxt,
                                        top: 1,
                                        columnsBlock: (data, ctxt) => {
                                            ctxt.AppendPartsLine(
                                                data.ColumnRowversion.GetNamePrefixed("@Current"),
                                                " = CAST(",
                                                data.ColumnRowversion.GetNameQ(),
                                                " as BIGINT)");
                                        },
                                        fromBlock: (data, ctxt) => {
                                            ctxt.AppendLine(data.GetNameQ());
                                        },
                                        whereBlock: (data, ctxt) => {
                                            ctxt.AppendList(
                                                data.PrimaryKeyColumns, // TODO FastPrimaryKeyColumns,
                                                (column, ctxt) => {
                                                    ctxt.AppendPartsLine(
                                                        ctxt.IfNotFirst(" AND "),
                                                        "(",
                                                        column.GetNamePrefixed("@"), " = ", column.GetNameQ(),
                                                        ")"
                                                        );
                                                });
                                        });
                                });
#endif
                            KnownTemplates.SqlSelect(
                                data.TableData,
                                ctxt,
                                top: 1,
                                columnsBlock: (data, ctxt) => {
                                    ctxt.AppendList(
                                        data.Columns,
                                        (column, ctxt) => {
                                            ctxt.AppendPartsLine(
                                                column.GetNamePrefixed("@Current"), " = ", column.GetNameQ(), ","
                                                );
                                        });
                                    ctxt.AppendPartsLine(
                                        data.ColumnRowversion.GetNamePrefixed("@Current"),
                                        " = CAST(",
                                        data.ColumnRowversion.GetNameQ(),
                                        " as BIGINT)");
                                },
                                fromBlock: (data, ctxt) => {
                                    ctxt.AppendLine(data.GetNameQ());
                                },
                                whereBlock: (data, ctxt) => {
                                    ctxt.AppendList(
                                        data.PrimaryKeyColumns, // TODO FastPrimaryKeyColumns,
                                        (column, ctxt) => {
                                            ctxt.AppendPartsLine(
                                                ctxt.IfNotFirst(" AND "),
                                                "(",
                                                column.GetNamePrefixed("@"), " = ", column.GetNameQ(),
                                                ")"
                                                );
                                        });
                                });
                            KnownTemplates.SqlIf(
                                data,
                                ctxt,
                                condition: (data, ctxt) => {
                                    ctxt.Append(
                                        KnownTemplates.SqlIsNull(
                                                data.TableData.ColumnRowversion.GetNamePrefixed("@Current")
                                            )
                                        );
                                },
                                thenBlock: (data, ctxt) => {

                                    KnownTemplates.SqlInsertValues(
                                        data,
                                        ctxt,
                                        target: data.TableData.GetNameQ(),
                                        nameBlock: (data, ctxt) => {
                                            ctxt.AppendList(
                                                data.TableData.Columns.Where(c => !c.Identity).ToList(),
                                                (column, ctxt) => {
                                                    ctxt.AppendPartsLine(column.GetNameQ(), ctxt.IfNotLast(","));
                                                }
                                                );
                                        },
                                        valuesBlock: (data, ctxt) => {
                                            ctxt.AppendList(
                                                data.TableData.Columns.Where(c => !c.Identity).ToList(),
                                                (column, ctxt) => {
                                                    ctxt.AppendPartsLine(column.GetNamePrefixed("@"), ctxt.IfNotLast(","));
                                                });
                                        });
                                    ctxt.AppendLine("SET @ResultValue = 1; /* Inserted */");
                                    /* History */
                                    KnownTemplates.SqlInsertValues(
                                        data,
                                        ctxt,
                                        target: data.TableHistory.GetNameQ(),
                                        nameBlock: (data, ctxt) => {
                                            ctxt.AppendList(
                                                data.ColumnPairs,
                                                (columnPair, ctxt) => {
                                                    ctxt.AppendPartsLine(
                                                        columnPair.columnHistory.GetNameQ(),
                                                        ","
                                                        );
                                                }
                                                );
                                            ctxt.AppendLine("[ValidFrom],");
                                            ctxt.AppendLine("[ValidTo]");
                                        },
                                        valuesBlock: (data, ctxt) => {
                                            ctxt.AppendList(
                                                data.ColumnPairs,
                                                (columnPair, ctxt) => {
                                                    ctxt.AppendPartsLine(
                                                        columnPair.columnData.GetNamePrefixed("@"),
                                                        ","
                                                        );
                                                }
                                                );
                                            ctxt.AppendLine("@ModifiedAt,");
                                            ctxt.AppendLine("CAST('3141-05-09T00:00:00Z' as datetimeoffset)");
                                        }
                                        );
                                },
                                elseBlock: (data, ctxt) => {
                                    KnownTemplates.SqlIf(
                                        data,
                                        ctxt,
                                        condition: (data, ctxt) => {
                                            var crv = data.TableData.ColumnRowversion.GetNamePrefixed("@");
                                            var crvCurrent = data.TableData.ColumnRowversion.GetNamePrefixed("@Current");
                                            ctxt.AppendPartsLine("(", crv, " <= 0)");
                                            ctxt.AppendPartsLine("OR ((0 < ", crv, ") AND (", crv, " = ", crvCurrent, "))");
                                        },
                                        thenBlock: (data, ctxt) => {
                                            KnownTemplates.SqlIf(
                                               data,
                                               ctxt,
                                               condition: (data, ctxt) => {
                                                   ctxt.AppendLine("EXISTS(");
                                                   var ctxt2 = ctxt.GetIndented();
                                                   var ctxt3 = ctxt2.GetIndented();
                                                   var columnsIncludedInCompare = data.TableData.Columns.Where(
                                                       c => c.ExtraInfo["ExcludeFromCompare"] switch {
                                                           true => false,
                                                           _ => true
                                                       }).ToList();
                                                   ctxt2.AppendLine("SELECT");
                                                   ctxt3.AppendList(
                                                       columnsIncludedInCompare,
                                                       (column, ctxt) => {
                                                           ctxt.AppendPartsLine(column.GetNamePrefixed("@"), ctxt.IfNotLast(","));
                                                       });
                                                   ctxt2.AppendLine("EXCEPT");
                                                   ctxt2.AppendLine("SELECT");
                                                   ctxt3.AppendList(
                                                       columnsIncludedInCompare,
                                                       (column, ctxt) => {
                                                           ctxt.AppendPartsLine(column.GetNamePrefixed("@Current"), ctxt.IfNotLast(","));
                                                       });
                                                   ctxt.Append(")");
                                               },
                                               thenBlock: (data, ctxt) => {
                                                   KnownTemplates.SqlUpdate(
                                                       data,
                                                       ctxt,
                                                       top: 1,
                                                       target: data.TableData.GetNameQ(),
                                                       setBlock: (data, ctxt) => {
                                                           ctxt.AppendList(
                                                               data.TableData.Columns.Where(column => (column.PrimaryKeyIndexPosition < 0) && (column.GetExtraInfo("ExcludeFromUpdate", false) == false)),
                                                               (column, ctxt) => {
                                                                   ctxt.AppendPartsLine(
                                                                       column.GetNameQ(),
                                                                       " = ",
                                                                       column.GetNamePrefixed("@"),
                                                                       ctxt.IfNotLast(","));
                                                               });
                                                       },
                                                       whereBlock: (data, ctxt) => {
                                                           ctxt.AppendList(data.TableData.PrimaryKeyColumns, (column, ctxt) => {
                                                               ctxt.AppendPartsLine(
                                                                   ctxt.IfNotFirst(" AND "),
                                                                   "(",
                                                                   column.GetNameQ(),
                                                                   " = ",
                                                                   column.GetNamePrefixed("@"),
                                                                   ")"
                                                                   );
                                                           });
                                                       });
                                                   ctxt.AppendLine("SET @ResultValue = 2; /* Updated */");
                                                   /* History */
                                                   KnownTemplates.SqlUpdate(
                                                       data,
                                                       ctxt,
                                                       top: 1,
                                                       target: data.TableHistory.GetNameQ(),
                                                       setBlock: (data, ctxt) => {
                                                           ctxt.AppendLine("[ValidTo] = @ModifiedAt");
                                                       },
                                                       whereBlock: (data, ctxt) => {
                                                           ctxt.AppendLine("([ValidTo] = CAST('3141-05-09T00:00:00Z' as datetimeoffset))");
                                                           ctxt.AppendList(
                                                                data.TableHistory.IndexPrimaryKey.Columns
                                                                    .Where(c =>
                                                                        !(string.Equals(c.Name, "ValidTo")
                                                                        || string.Equals(c.Name, "ValidFrom"))
                                                                    ),
                                                                (column, ctxt) => {
                                                                    //ctxt.Append(ctxt.IfNotFirst("AND "));
                                                                    if (column.Name == "OperationId") {
                                                                        ctxt.AppendLine("AND ([OperationId] = @CurrentOperationId)");
                                                                    } else {
                                                                        ctxt.AppendPartsLine("AND (", column.GetNameQ(), " = ", column.GetNamePrefixed("@"), ")");
                                                                    }
                                                                });
                                                       }
                                                       );
                                                   KnownTemplates.SqlInsertValues(
                                                       data,
                                                       ctxt,
                                                       target: data.TableHistory.GetNameQ(),
                                                       nameBlock: (data, ctxt) => {
                                                           ctxt.AppendList(
                                                               data.ColumnPairs,
                                                               (columnPair, ctxt) => {
                                                                   ctxt.AppendPartsLine(
                                                                       columnPair.columnHistory.GetNameQ(),
                                                                       ","
                                                                       );
                                                               }
                                                               );
                                                           ctxt.AppendLine("[ValidFrom],");
                                                           ctxt.AppendLine("[ValidTo]");
                                                       },
                                                       valuesBlock: (data, ctxt) => {
                                                           ctxt.AppendList(
                                                               data.ColumnPairs,
                                                               (columnPair, ctxt) => {
                                                                   ctxt.AppendPartsLine(
                                                                       columnPair.columnData.GetNamePrefixed("@"),
                                                                       ","
                                                                       );
                                                               }
                                                               );
                                                           ctxt.AppendLine("@ModifiedAt,");
                                                           ctxt.AppendLine("CAST('3141-05-09T00:00:00Z' as datetimeoffset)");
                                                       }
                                                       );
                                               },
                                               elseBlock: (data, ctxt) => {
                                                   ctxt.AppendLine("SET @ResultValue = 0; /* NoNeedToUpdate */");
                                               });
                                        },
                                        elseBlock: (data, ctxt) => {
                                            ctxt.AppendLine("SET @ResultValue = -1 /* RowVersionMismatch */;");
                                        });
                                });
                            KnownTemplates.SqlSelect(
                                data,
                                ctxt,
                                top: 1,
                                columnsBlock: (data, ctxt) => {
                                    ctxt.AppendList(
                                        data.TableData.Columns,
                                        (column, ctxt) => {
                                            ctxt.AppendPartsLine(
                                                column.GetNameQ(), ","
                                                );
                                        });
                                    ctxt.AppendLine(
                                        $"{data.TableData.ColumnRowversion.GetNameQ()} = CAST({data.TableData.ColumnRowversion.GetNameQ()} as BIGINT)");
                                },
                                fromBlock: (data, ctxt) => {
                                    ctxt.AppendLine(data.TableData.GetNameQ());
                                },
                                whereBlock: (data, ctxt) => {
                                    ctxt.AppendList(
                                        data.TableData.PrimaryKeyColumns,
                                        (column, ctxt) => {
                                            ctxt.AppendPartsLine(
                                                ctxt.IfNotFirst(" AND "),
                                                "(",
                                                column.GetNamePrefixed("@"), " = ", column.GetNameQ(),
                                                ")"
                                                );
                                        });
                                });
                            ctxt.AppendLine("SELECT ResultValue = @ResultValue, [Message] = '';");
                        }
                    );
                });

            this.DeletePKTempateParameter = new RenderTemplate<TableDataHistory>(
                NameFn: tdh => $"DeletePKTempateParameter.{tdh.TableData.GetNameQ()}",
                Render: (data, ctxt) => {
                    ctxt.AppendList(data.TableData.PrimaryKeyColumns, (column, ctxt) => {
                        ctxt.AppendPartsLine(
                            column.GetNamePrefixed("@"),
                            " ",
                            column.GetParameterSqlDataType(),
                            ","
                            );

                    });
                    ctxt.AppendPartsLine("@OperationId uniqueidentifier,");
                    ctxt.AppendPartsLine("@ModifiedAt datetimeoffset,");
                    ctxt.AppendPartsLine("@ModifiedBy uniqueidentifier,");
                    ctxt.AppendPartsLine(
                            data.TableData.ColumnRowversion.GetNamePrefixed("@"),
                            " ",
                            data.TableData.ColumnRowversion.GetParameterSqlDataType()
                            );
                });
            this.DeletePKTempate = new RenderTemplate<TableDataHistory>(
                FileNameFn: RenderTemplateExtentsions.GetFileNameBind<TableDataHistory>(@"[Schema]\StoredProcedures\[Schema].[Name]DeletePK.sql"),
                Render: (data, ctxt) => {
                    KnownTemplates.SqlCreateProcedure(
                        data,
                        ctxt,
                        schema: data.TableData.Table.Schema,
                        name: $"{data.TableData.Table.Name}DeletePK",
                        parameter: (data, ctxt) => {
                            this.DeletePKTempateParameter.Render(data, ctxt);
                        },
                        bodyBlock: (data, ctxt) => {

                            ctxt.RenderTemplate(
                                (columns: data.TableData.Columns, columnRowVersion: data.TableData.ColumnRowversion, prefix: "@Current"),
                                this.KnownTemplates.ColumnsAsDeclareParameterWithRowVersion
                                );

                            ctxt.AppendLine("DECLARE @Result AS TABLE (");
                            ctxt.GetIndented().AppendList(
                                data.TableData.PrimaryKeyColumns,
                                (column, ctxt) => {
                                    var sqlDataType = column.GetSqlDataType();
                                    var name = column.GetNameQ();
                                    ctxt.AppendPartsLine(name, " ", sqlDataType, ctxt.IfNotLast(","));
                                });
                            ctxt.AppendLine(");");

                            /*
                            foreach (var foreignKeysReferenced in data.TableData.ForeignKeysReferenced) {
                                ctxt.AppendLine("--");
                                ctxt.AppendLine($"-- {foreignKeysReferenced.IndexReferenced.Name}");
                                ctxt.AppendLine($"-- TableInfo: {foreignKeysReferenced.TableInfo.GetNameQ()}");
                                ctxt.AppendLine($"-- TableInfoReferenced: {foreignKeysReferenced.TableInfoReferenced.GetNameQ()}");

                            }
                            */

                            ctxt.AppendLine("");
                            KnownTemplates.SqlSelect(
                                        data.TableData,
                                        ctxt,
                                        top: 1,
                                        columnsBlock: (data, ctxt) => {
                                            ctxt.AppendList(
                                                data.Columns,
                                                (column, ctxt) => {
                                                    ctxt.AppendPartsLine(
                                                        column.GetNamePrefixed("@Current"), " = ", column.GetNameQ(), ","
                                                        );
                                                });
                                            ctxt.AppendPartsLine(
                                                data.ColumnRowversion.GetNamePrefixed("@Current"),
                                                " = CAST(",
                                                data.ColumnRowversion.GetNameQ(),
                                                " as BIGINT)");
                                        },
                                        fromBlock: (data, ctxt) => {
                                            ctxt.AppendLine(data.GetNameQ());
                                        },
                                        whereBlock: (data, ctxt) => {
                                            ctxt.AppendList(
                                                data.PrimaryKeyColumns, // TODO FastPrimaryKeyColumns,
                                                (column, ctxt) => {
                                                    ctxt.AppendPartsLine(
                                                        ctxt.IfNotFirst(" AND "),
                                                        "(",
                                                        column.GetNamePrefixed("@"), " = ", column.GetNameQ(),
                                                        ")"
                                                        );
                                                });
                                        });

                            ctxt.AppendLine("");

                            ctxt.AppendLine($"DELETE FROM {data.TableData.GetNameQ()}");
                            var ctxtIndented1 = ctxt.GetIndented();
                            var ctxtIndented2 = ctxtIndented1.GetIndented();
                            ctxtIndented1.AppendLine("OUTPUT");
                            ctxtIndented2.AppendList(
                                data.TableData.PrimaryKeyColumns,
                                (column, ctxt) => {
                                    ctxt.AppendPartsLine(
                                        "DELETED.", column.GetNameQ(), ctxt.IfNotLast(",")
                                        );
                                });
                            ctxtIndented1.AppendLine("INTO @Result");

                            ctxtIndented1.AppendList(
                                data.TableData.PrimaryKeyColumns,
                                (column, ctxt) => {
                                    ctxt.AppendPartsLine(
                                        ctxt.SwitchFirst("WHERE ", "    AND "),
                                        "(",
                                        column.GetNamePrefixed("@"), " = ", column.GetNameQ(),
                                        ")"
                                        );
                                });
                            ctxtIndented1.AppendLine(";");

                            ctxt.AppendLine("");

                            KnownTemplates.SqlIf(
                                data,
                                ctxt,
                                condition: (data, ctxt) => {
                                    var ctxtIndented1 = ctxt.GetIndented();

                                    ctxt.AppendLine("EXISTS(");
                                    ctxt.AppendLine("SELECT");
                                    ctxtIndented1.AppendList(
                                        data.TableData.PrimaryKeyColumns,
                                        (column, ctxt) => {
                                            ctxt.AppendPartsLine(
                                                column.GetNameQ(), ctxt.IfNotLast(",")
                                                );
                                        });
                                    ctxtIndented1.AppendLine("FROM @Result");
                                    ctxt.AppendLine(")");
                                },
                                thenBlock: (data, ctxt) => {

                                    KnownTemplates.SqlUpdate(
                                        data,
                                        ctxt,
                                        top: 1,
                                        target: data.TableHistory.GetNameQ(),
                                        setBlock: (data, ctxt) => {
                                            ctxt.AppendLine("[ValidTo] = @ModifiedAt");
                                        },
                                        whereBlock: (data, ctxt) => {
                                            var ctxtIndented1 = ctxt.GetIndented();

                                            ctxtIndented1.AppendLine("([OperationId] = @CurrentOperationId)");
                                            ctxtIndented1.AppendLine("AND ([ValidTo] = CAST('3141-05-09T00:00:00Z' as datetimeoffset))");
                                            ctxtIndented1.AppendList(
                                                data.TableData.PrimaryKeyColumns,
                                                (column, ctxt) => {
                                                    ctxt.AppendPartsLine(
                                                        "    AND ",
                                                        "(",
                                                        column.GetNamePrefixed("@"), " = ", column.GetNameQ(),
                                                        ")"
                                                        );
                                                });
                                        });

                                    KnownTemplates.SqlInsertValues(
                                        data,
                                        ctxt,
                                        target: data.TableHistory.GetNameQ(),
                                        nameBlock: (data, ctxt) => {
                                            ctxt.AppendList(
                                                data.ColumnPairs,
                                                (columnPair, ctxt) => {
                                                    ctxt.AppendPartsLine(
                                                        columnPair.columnHistory.GetNameQ(),
                                                        ","
                                                        );
                                                }
                                                );
                                            ctxt.AppendLine("[ValidFrom],");
                                            ctxt.AppendLine("[ValidTo]");
                                        },
                                        valuesBlock: (data, ctxt) => {
                                            ctxt.AppendList(
                                                data.ColumnPairs,
                                                (columnPair, ctxt) => {
                                                    string name = columnPair.columnData.GetNamePrefixed("@Current");
                                                    if (name == "@CurrentOperationId") {
                                                        name = "@OperationId";
                                                    } else if (name == "@CurrentModifiedAt") {
                                                        name = "@ModifiedAt";
                                                    } else if (name == "@CurrentModifiedBy") {
                                                        name = "@ModifiedBy";
                                                    }
                                                    ctxt.AppendPartsLine(
                                                        name,
                                                        ","
                                                        );
                                                }
                                                );
                                            ctxt.AppendLine("@ModifiedAt,");
                                            ctxt.AppendLine("@ModifiedAt");
                                        }
                                        );

                                });

                            ctxt.AppendLine("SELECT");
                            ctxtIndented1.AppendList(
                                data.TableData.PrimaryKeyColumns,
                                (column, ctxt) => {
                                    ctxt.AppendPartsLine(
                                        column.GetNameQ(), ctxt.IfNotLast(",")
                                        );
                                });
                            ctxtIndented1.AppendLine("FROM @Result");
                            ctxtIndented1.AppendLine(";");

                        });
                });
            //
        }

        public override ConfigurationBound Build(DatabaseInfo databaseInfo) {
            databaseInfo = new DatabaseInfo() {
                Tables = databaseInfo.Tables.Where(t => !t.GetNameQ().StartsWith("[dbo][Orleans")).ToList(),
            };
            var result = base.Build(databaseInfo);
            var stringComparer = System.StringComparer.OrdinalIgnoreCase;
            var hsExcludeFromCompare = new HashSet<string>(stringComparer);
            hsExcludeFromCompare.Add("OperationId");
            hsExcludeFromCompare.Add("CreatedAt");
            hsExcludeFromCompare.Add("ModifiedAt");
            hsExcludeFromCompare.Add("SerialVersion");

            foreach (var t in databaseInfo.Tables.Where(
                t => IsADataTable(t))) {
                foreach (var column in t.Columns) {
                    var excludeFromCompare = hsExcludeFromCompare.Contains(column.Name);
                    column.ExtraInfo["ExcludeFromCompare"] = excludeFromCompare;
                    column.ExtraInfo["ExcludeFromUpdate"] = (stringComparer.Equals(column.Name, "CreatedAt"));
                }
            }

            foreach (var tableInfo in databaseInfo.Tables) {
                if (tableInfo.ColumnRowversion is not null) {
                    tableInfo.ColumnRowversion.ParameterSqlDataType = "BIGINT";
                    tableInfo.ColumnRowversion.ReadExpression = (sourceAlias) => {
                        var n = tableInfo.ColumnRowversion.GetNameQ(sourceAlias);
                        return $"CAST({n} AS BIGINT)";
                    };
                }
            }

            var tablesInsertOnly = databaseInfo.Tables.Where(
                t => IsOperationTable(t)
                ).ToList();

            var tablesHistory = databaseInfo.Tables.Where(
                t => !IsOperationTable(t) && IsAHistoryTable(t)
                ).ToList();

            var tablesUpdate = databaseInfo.Tables.Where(
                t => !IsOperationTable(t) && !IsAHistoryTable(t)
                ).ToList();

            var tablesUpdatePaired = tablesUpdate.Join(
                    tablesHistory,
                    o => o.Name,
                    i => i.Name.EndsWith("History") ? i.Name.Substring(0, i.Name.Length - 7) : null,
                    (tableInfoData, tableInfoHistory) => new TableDataHistory(
                        tableInfoData,
                        tableInfoHistory,
                        tableInfoData.Columns.Join(
                            tableInfoHistory.Columns,
                            o => o.Name,
                            i => i.Name,
                            (o, i) => (columnData: o, columnHistory: i)
                            ).ToList()
                    )
                ).ToList();

            var tablesNotHistory = databaseInfo.Tables.Where(
                t => !IsAHistoryTable(t)
                ).ToList();

            var tablesDataWithFKReferenced = databaseInfo.Tables.Select(
                    tableInfo => tableInfo.ForeignKeysReferenced.Where(fk => !IsADataTable(fk.TableInfo))
                    .OrderBy(t => t.TableInfo.GetNameQ())
                    .ToList()
                ).Where(
                    fks => fks.Count > 0
                ).Select(
                    fks => new TableDataFK(fks[0].TableInfoReferenced, fks)
                ).ToList();


            result.AddRenderBindings(
                    "SelectPKTempate",
                    tablesNotHistory.Where(t => t.GetNameQ() != "[dbo].[Project]")
                    .Select(tableInfo => new TableBinding(tableInfo, this.SelectPKTempate)));


            //result.RenderBindings.AddRange(
            //        databaseInfo.Tables.Where(t => t.Name == "ExternalSource")
            //        .Select(tableInfo => new TableBinding(tableInfo, this.SelectAtTimeTempate))
            //    );

            //result.RenderBindings.AddRange(
            //        databaseInfo.ForeignKey
            //        .Select(foreignKey => new TemplateBinding<ForeignKeyInfo>(foreignKey, this.SelectByReferencedPKTempate))
            //    );

            result.AddRenderBindings(
                nameof(UpdateTempate),
                tablesUpdatePaired
                    .Select(
                        t => new DataTemplateBinding<TableDataHistory>(
                            t,
                            t => t.TableData,
                            this.UpdateTempate)));

            result.AddRenderBindings(
                nameof(DeletePKTempate),
                tablesUpdatePaired
                    .Select(
                        t => new DataTemplateBinding<TableDataHistory>(
                            t,
                            t => t.TableData,
                            this.DeletePKTempate)));

            result.AddReplacementBindings(
                nameof(SelectColumnsParameterPKTempate),
                databaseInfo.Tables.Select(
                    t => new TableBinding(t, this.SelectColumnsParameterPKTempate)));

            result.AddReplacementBindings(
                nameof(ConditionColumnsParameterPKTempate),
                databaseInfo.Tables.Select(
                    t => new TableBinding(t, this.ConditionColumnsParameterPKTempate)));

            result.AddReplacementBindings(
                nameof(AtTableResultPKTempate),
                databaseInfo.Tables.Select(
                    t => new TableBinding(t, this.AtTableResultPKTempate)));

            result.AddReplacementBindings(
                nameof(AtTableResultTempate),
                databaseInfo.Tables.Select(
                    t => new TableBinding(t, this.AtTableResultTempate)));

            result.AddReplacementBindings(
                nameof(SelectAtTableResultTemplate),
                databaseInfo.Tables.Select(
                    t => new TableBinding(t, this.SelectAtTableResultTemplate)));

            result.AddReplacementBindings(
                nameof(InsertIntoTableOutputAtTableResultValuesParameterTemplate),
                databaseInfo.Tables.Select(
                    t => new TableBinding(t, this.InsertIntoTableOutputAtTableResultValuesParameterTemplate)));
            
            result.AddReplacementBindings(
                nameof(InsertIntoTableValuesParameterTemplate),
                databaseInfo.Tables.Select(
                    t => new TableBinding(t, this.InsertIntoTableValuesParameterTemplate)));

            result.AddReplacementBindings(
                nameof(SelectPKTempateBody),
                databaseInfo.Tables.Select(
                    t => new TableBinding(t, this.SelectPKTempateBody)));

            result.AddReplacementBindings(
                nameof(DeletePKTempateParameter),
                tablesUpdatePaired
                    .Select(
                        t => new DataTemplateBinding<TableDataHistory>(
                            t,
                            t => t.TableData,
                            this.DeletePKTempateParameter)));


            return result;
        }

        private static bool IsADataTable(TableInfo t) {
            return !(IsOperationTable(t) || IsRequestLogTable(t) || IsAHistoryTable(t));
        }

        private static bool IsAHistoryTable(TableInfo t) {
            return string.Equals(t.Schema, "history", System.StringComparison.Ordinal);
        }

        private static bool IsOperationTable(TableInfo t) {
            return string.Equals(t.GetNameQ(), "[dbo].[Operation]", System.StringComparison.Ordinal);
        }

        private static bool IsRequestLogTable(TableInfo t) {
            return string.Equals(t.GetNameQ(), "[dbo].[RequestLog]", System.StringComparison.Ordinal);
        }
    }
}
