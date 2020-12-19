import { IDetailsColumnStyles } from "office-ui-fabric-react/lib/DetailsList";
import { IIconProps } from "office-ui-fabric-react";
import { IStackStyles, mergeStyleSets } from "@fluentui/react";

export const classNames = mergeStyleSets({
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

export const addIcon: IIconProps = { iconName: "Add" };

export const stackStyles: Partial<IStackStyles> = { root: { width: "75rem" } };

export const detailsListStyles: Partial<IDetailsColumnStyles> = {
  root: { textAlign: "right" },
};
