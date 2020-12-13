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
} from "@fluentui/react";
import Dialog from "@material-ui/core/Dialog";
import DialogContent from "@material-ui/core/DialogContent";
import {
  ISupplierProposalFormDetails,
  SupplierProposalForm,
} from "./SupplierProposalForm";
import { SuppliersSurvey } from "./SupplierSurvey";

export interface ISuppliersListItem {
  key: number;
  name: string;
  Price: number;
  MinimumUnits: number;
  Date: string;
  ProgressBar: JSX.Element;
  Description?: string;
}

export interface ISuppliersListState {
  items: ISuppliersListItem[];
}

export interface ISuppliersSectionProps {
  requestedItems: number;
  groupExpirationDate: Date;
}

const classNames = mergeStyleSets({
  fileIconCell: {
    marginLeft: "1.4rem",
  },
  fileIconCellDate: {
    marginLeft: "-2rem",
  },
  fileIconCellDescription: {
    marginLeft: "1.8rem",
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
        label={
          170 + i - 170 > 0 ? `${170 + i - 170} units to complete` : "Complete"
        }
        percentComplete={170 / (170 + i)}
      />
    ),
    Description:
      "Computer online store KSP offer a wide range of computers. Easy filters help you to choose the computer is most suitable for your needs. As for the price of computers, we are closely watching the computers market in Israel and Netanya, and therefore offer our customers only the best prices, allowing you to buy cheap computer. All our products, including computers, are certified and have the official warranty from the manufacturer.",
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
    maxWidth: 100,
    isResizable: true,
    styles: { cellTitle: { marginLeft: "1.3rem" } },
    className: classNames.fileIconCell,
  },
  {
    key: "column3",
    name: "Minimum units",
    fieldName: "MinimumUnits",
    minWidth: 70,
    maxWidth: 100,
    isResizable: true,
  },
  {
    key: "column5",
    name: "Date",
    fieldName: "Date",
    minWidth: 140,
    maxWidth: 190,
    isResizable: true,
    className: classNames.fileIconCellDate,
  },
  {
    key: "column6",
    name: "ProgressBar",
    fieldName: "ProgressBar",
    minWidth: 150,
    maxWidth: 150,
    isResizable: true,
    styles: { cellTitle: { marginLeft: "0.4rem" } },
  },
  {
    key: "column7",
    name: "Description",
    fieldName: "Description",
    minWidth: 150,
    maxWidth: 150,
    isResizable: true,
    isMultiline: true,
    className: classNames.fileIconCellDescription,
    styles: { cellTitle: { marginLeft: "1rem" } },
  },
];

export const addIcon: IIconProps = { iconName: "Add" };

export const stackStyles: Partial<IStackStyles> = { root: { width: "65rem" } };

export const detailsListStyles: Partial<IDetailsColumnStyles> = {
  root: { textAlign: "right" },
};

export const SuppliersSection: React.FunctionComponent<ISuppliersSectionProps> = ({
  requestedItems,
  groupExpirationDate,
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
    supplierProposalFormDetails: ISupplierProposalFormDetails
  ): void => {
    const stam: ISuppliersListItem = {
      key: listItems.length + 1,
      name: supplierProposalFormDetails.supplierName,
      Price: supplierProposalFormDetails.proposedPrice as number,
      MinimumUnits: supplierProposalFormDetails.minimumUnits as number,
      Date: supplierProposalFormDetails.date,
      ProgressBar: (
        //TODO: how to present the minimumUnits-requestedItems avoid injection
        <ProgressIndicator
          label={
            (supplierProposalFormDetails.minimumUnits as number) -
              requestedItems >
            0
              ? `${
                  (supplierProposalFormDetails.minimumUnits as number) -
                  requestedItems
                } units to complete`
              : "Complete"
          }
          percentComplete={
            requestedItems /
            (supplierProposalFormDetails.minimumUnits as number)
          }
        />
      ),
      Description: supplierProposalFormDetails.description,
    };
    setOpen(false);
    setListItems((listItems) => [stam, ...listItems]);
  };
  return new Date().getTime() < groupExpirationDate.getTime() ? (
    <Stack styles={stackStyles}>
      <ActionButton
        iconProps={addIcon}
        allowDisabledFocus
        disabled={false}
        checked={false}
        onClick={handleClickOpen}
      >
        Add a new Supplier proposal
      </ActionButton>
      <Dialog
        open={open}
        onClose={handleClose}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
      >
        <DialogContent>
          <SupplierProposalForm
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
  ) : (
    <Stack>
      <SuppliersSurvey
        supliersNames={listItems.map((supplierProposal) => {
          return {
            key: String(supplierProposal.key),
            text: supplierProposal.name,
          };
        })}
      />
    </Stack>
  );
};
