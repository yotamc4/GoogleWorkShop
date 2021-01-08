import * as React from "react";
import { TextField } from "office-ui-fabric-react/lib/TextField";
import { Image, ImageFit } from "office-ui-fabric-react/lib/Image";
import { classNames } from "./GroupListStyles";
import {
  DetailsList,
  DetailsListLayoutMode,
  IColumn,
  Link,
  SelectionMode,
  Stack,
} from "@fluentui/react";
import { Bid } from "../Modal/GroupDetails";
import { PhasesName } from "../Modal/ProductDetails";

export const GroupsList: React.FunctionComponent<{
  groups: Bid[];
}> = (props) => {
  const [currentGroups, setCurrentGroups] = React.useState<Bid[]>(props.groups);
  const [currentColumns, setCurrentColumns] = React.useState<IColumn[]>([]);

  const onColumnClick = (
    ev: React.MouseEvent<HTMLElement>,
    column: IColumn
  ): void => {
    const newColumns: IColumn[] = columns.slice();
    const currColumn: IColumn = newColumns.filter(
      (currCol) => column.key === currCol.key
    )[0];

    newColumns.forEach((newCol: IColumn) => {
      if (newCol === currColumn) {
        currColumn.isSortedDescending = !currColumn.isSortedDescending;
        currColumn.isSorted = true;
      } else {
        newCol.isSorted = false;
        newCol.isSortedDescending = true;
      }
    });

    const newItems = _copyAndSort(
      currentGroups,
      currColumn.fieldName!,
      currColumn.isSortedDescending
    );

    setCurrentGroups(newItems);
    setCurrentColumns(newColumns);
  };

  const columns: IColumn[] = [
    {
      key: "column1",
      name: "Group",
      minWidth: 120,
      maxWidth: 150,
      data: "string",
      onRender: (group: Bid) => {
        return (
          <Link href={`/products/${group.id}`}>{group.product!.name}</Link>
        );
      },
      isResizable: true,
    },
    {
      key: "column2",
      name: "Expiration Date",
      minWidth: 120,
      maxWidth: 130,
      isSorted: true,
      isSortedDescending: false,
      fieldName: "expirationDate",
      onColumnClick: onColumnClick,
      onRender: (group: Bid) => {
        return <span>{group.expirationDate.toLocaleDateString()}</span>;
      },
    },
    {
      key: "column3",
      name: "Creation Date",
      minWidth: 120,
      maxWidth: 130,
      isSorted: true,
      isSortedDescending: false,
      fieldName: "creationDate",
      onColumnClick: onColumnClick,
      onRender: (group: Bid) => {
        return <span>{group.creationDate.toLocaleDateString()}</span>;
      },
    },
    {
      key: "column4",
      name: "Group stage",
      minWidth: 120,
      maxWidth: 150,
      isSorted: true,
      isSortedDescending: false,
      fieldName: "phase",
      onColumnClick: onColumnClick,
      onRender: (group: Bid) => {
        return <span>{PhasesName.get(group.phase)}</span>;
      },
    },
  ];

  const resultCountText =
    currentGroups.length === props.groups.length
      ? ""
      : ` (${currentGroups.length} of ${props.groups.length} shown)`;

  const onFilterChanged = (
    event: React.FormEvent<HTMLInputElement | HTMLTextAreaElement>
  ): void => {
    setCurrentGroups(
      props.groups.filter(
        (group) =>
          group
            .product!.name.toLowerCase()
            .indexOf((event.target as HTMLInputElement).value.toLowerCase()) >=
          0
      )
    );
  };

  React.useEffect(() => {
    setCurrentColumns(columns);
  }, []);

  return (
    <Stack tokens={{ childrenGap: "1rem" }} data-is-scrollable>
      <TextField
        label={"Filter by name:" + resultCountText}
        onChange={onFilterChanged}
      />
      <DetailsList
        items={currentGroups}
        columns={currentColumns}
        layoutMode={DetailsListLayoutMode.justified}
        isHeaderVisible={true}
        selectionMode={SelectionMode.none}
      />
    </Stack>
  );
};

function _copyAndSort<T>(
  items: T[],
  columnKey: string,
  isSortedDescending?: boolean
): T[] {
  const key = columnKey as keyof T;
  return items
    .slice(0)
    .sort((a: T, b: T) =>
      (isSortedDescending ? a[key] < b[key] : a[key] > b[key]) ? 1 : -1
    );
}
