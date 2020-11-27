import * as React from "react";
import {
  DetailsList,
  IColumn,
  IDetailsColumnStyles,
} from "office-ui-fabric-react/lib/DetailsList";
import { Stack } from "office-ui-fabric-react";
import { ActionButton, IIconProps } from "office-ui-fabric-react";
import {
  IStackStyles,
  mergeStyleSets,
  SelectionMode,
  ProgressIndicator,
  PrimaryButton,
  DefaultButton,
  TextField,
} from "@fluentui/react";
import Dialog from "@material-ui/core/Dialog";
import DialogContent from "@material-ui/core/DialogContent";
import { NewProposalForm } from "./NewProposalForm";
export interface ISuppliersListItem {
  key: number;
  name: string;
  Price: number;
  MinimumUnits: number;
  Date: string;
  ProgressBar: JSX.Element;
}

export interface ISuppliersListState {
  items: ISuppliersListItem[];
}

export interface ISuppliersSectionProps {
  requestedItems: number;
}

const classNames = mergeStyleSets({
  fileIconCell: {
    marginLeft: "1.4rem",
  },
  fileIconCell2: {
    marginLeft: "-2rem",
  },
});

export let _allItems: ISuppliersListItem[] = [];

export let _columns: IColumn[] = [];

for (let i = 0; i < 300; i += 100) {
  _allItems.push({
    key: i,
    name: "KSP computers and cellular",
    Price: 10000,
    MinimumUnits: 170 + i,
    Date: Date(),
    ProgressBar: (
      <ProgressIndicator
        label="70 units to complete"
        percentComplete={170 / (170 + i)}
      />
    ),
  });
}

_columns = [
  {
    key: "column1",
    name: "Supplier name",
    fieldName: "name",
    minWidth: 100,
    maxWidth: 150,
    isResizable: true,
  },
  {
    key: "column2",
    name: "Price",
    fieldName: "Price",
    minWidth: 70,
    maxWidth: 120,
    isResizable: true,
    styles: { cellTitle: { marginLeft: "1.3rem" } },
    className: classNames.fileIconCell,
  },
  {
    key: "column3",
    name: "Minimum units",
    fieldName: "MinimumUnits",
    minWidth: 70,
    maxWidth: 120,
    isResizable: true,
  },
  {
    key: "column5",
    name: "Date",
    fieldName: "Date",
    minWidth: 140,
    maxWidth: 140,
    isResizable: true,
    className: classNames.fileIconCell2,
  },
  {
    key: "column6",
    name: "ProgressBar",
    fieldName: "ProgressBar",
    minWidth: 100,
    maxWidth: 100,
    isResizable: true,
    styles: { cellTitle: { marginLeft: "1rem" } },
  },
];

export const addIcon: IIconProps = { iconName: "Add" };

export const stackStyles: Partial<IStackStyles> = { root: { width: "50rem" } };

export const detailsListStyles: Partial<IDetailsColumnStyles> = {
  root: { textAlign: "right" },
};

export const SuppliersSection: React.FunctionComponent<ISuppliersSectionProps> = ({
  requestedItems,
}) => {
  const [open, setOpen] = React.useState(false);
  const [listItems, setListItems] = React.useState<ISuppliersListItem[]>(
    _allItems
  );

  const handleClickOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  const addPropposalToSupplierList = (
    price: number,
    minimumUnits: number
  ): void => {
    const stam: ISuppliersListItem = {
      key: listItems.length + 1,
      name: `Ofek's store`,
      Price: price,
      MinimumUnits: minimumUnits,
      Date: Date(),
      ProgressBar: (
        <ProgressIndicator
          label="70 units to complete"
          percentComplete={requestedItems / minimumUnits}
        />
      ),
    };
    setOpen(false);
    setListItems((listItems) => [stam, ...listItems]);
  };
  return (
    <Stack styles={stackStyles}>
      <ActionButton
        iconProps={addIcon}
        allowDisabledFocus
        disabled={false}
        checked={false}
        onClick={handleClickOpen}
      >
        Add a new proposal
      </ActionButton>
      <Dialog
        open={open}
        onClose={handleClose}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
      >
        <DialogContent>
          <NewProposalForm
            addPropposalToSupplierList={addPropposalToSupplierList}
            handleClose={handleClose}
          />
        </DialogContent>
      </Dialog>
      <></>
      <DetailsList
        items={listItems}
        columns={_columns}
        selectionMode={SelectionMode.none}
      />
    </Stack>
  );
};
